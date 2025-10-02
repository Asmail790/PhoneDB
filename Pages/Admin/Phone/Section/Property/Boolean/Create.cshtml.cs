using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Section.Property.Boolean;

public class Create : PageModel
{
    private PhoneDbContext _context;
    private DataBase.PhoneDbLoadHelper _phoneDbLoadHelper;


    public Create(PhoneDbContext context)
    {
        _context = context;
        _phoneDbLoadHelper = new PhoneDbLoadHelper(context);
    }

    [Required]
    [BindProperty(SupportsGet = true)]
    public int? SectionId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public int? PropertyTypeId { get; set; }

    [BindProperty()] public bool Value { get; set; } = false;

    public DataBase.Section? Section { get; set; } = null;
    public DataBase.BooleanPropertyType? PropertyType { get; set; } = null;

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (SectionId is null || PropertyTypeId is null)
        {
            throw new InvalidOperationException();
        }

        var sectionId = SectionId.Value;
        var propertyTypeId = PropertyTypeId.Value;


        await _phoneDbLoadHelper.LoadAllSectionTypesIntoContext();
        await _phoneDbLoadHelper.LoadAllBooleanPropertyTypesIntoContext();

        var section = await _context.Sections
            .Include(section => section.SectionType)
            .Include(section => section.Phone)
            .Where(item => item.Id.Equals(sectionId)).ToListAsync().FirstOrNull();

        if (section is null)
        {
            ModelState.AddModelError(nameof(Create.SectionId), "sectionId not found");
            return;
        }

        Section = section;

        var propertyType = await _context.BooleanPropertyTypes.Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync()
            .FirstOrNull();

        if (propertyType is null)
        {
            ModelState.AddModelError(nameof(Create.SectionId), "propertyId not found");
            return;
        }

        PropertyType = propertyType;

        bool propertyTypeIsNotIncludedInSectionType = !await _context.Sections.AnyAsync(item =>
            item.Id.Equals(sectionId) &&
            item.SectionType.BooleanPropertyTypes.Any(type => type.Id.Equals(propertyTypeId)));

        if (propertyTypeIsNotIncludedInSectionType)
        {
            ModelState.AddModelError(nameof(Create.SectionId), "property type Id can not be inserted ");
            return;
        }

        bool propertyWithSameInNameAndPropertyExist = await _context.BooleanProperties.AnyAsync(item =>
            item.SectionId.Equals(sectionId) && item.BooleanPropertyTypeId.Equals(propertyTypeId)
        );

        if (propertyWithSameInNameAndPropertyExist)
        {
            ModelState.AddModelError(nameof(Create.PropertyTypeId),
                "A property with same propertyType exist already in section database");
        }
    }

   

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Section is null || PropertyType is null)
        {
            throw new InvalidOperationException();
        }


        var item = new BooleanProperty()
        {
            Section = Section,
            BoolData = Value,
            BooleanPropertyType = PropertyType
        };

        _context.BooleanProperties.Add(item);
        return await PropertyPageModelUtilties.SaveAndRedirectIfSucceses(
            this,
            _context,
            ModelState,
            Section.PhoneId
        );
    }
}