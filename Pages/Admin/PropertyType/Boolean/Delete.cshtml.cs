using DataBase;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Boolean;

public class Delete(PhoneDbContext context) : DeletePropertyType<BooleanPropertyType>(context)
{
    protected override async Task<BooleanPropertyType?> FindPropertyType(int propertyTypeId)
    {
       return await Context.BooleanPropertyTypes
           .Include(item => item.SectionType)
           .Where(item => item.Id.Equals(propertyTypeId))
           .ToListAsync().FirstOrNull();
    }

    protected override void RemovePropertyType(BooleanPropertyType propertyType)
    {
        Context.BooleanPropertyTypes.Remove(propertyType);
    }
}
/*public class Delete : PageModel
{
    private readonly PhoneDbContext _context;

    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyTypeId { get; set; }

    public BooleanPropertyType? PropertyType { get; set; }


    public Delete(PhoneDbContext context)
    {
        _context = context;
    }

    private async Task ValidateDeleteRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (PropertyTypeId is null)
        {
            throw new InvalidOperationException();
        }

        var propertyTypeId = PropertyTypeId.Value;

        var propertyType = await _context.BooleanPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
        if (propertyType is null)
        {
            ModelState.AddModelError(nameof(PropertyTypeId),
                $"The property type with id {propertyTypeId} is not found.");
            return;
        }

        PropertyType = propertyType;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateDeleteRequest();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateDeleteRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PropertyType is null)
        {
            throw new InvalidOperationException();
        }

        _context.BooleanPropertyTypes.Remove(PropertyType);

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState,
            RedirectToPage(
                "/Admin/SectionType/Edit", new
                {
                    sectionTypeId = PropertyType.SectionType.Id
                }));
    }
}*/