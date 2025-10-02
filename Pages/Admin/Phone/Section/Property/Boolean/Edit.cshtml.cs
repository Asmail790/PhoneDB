using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Section.Property;


public class Edit : PageModel
{
    private PhoneDbContext _context;

    public Edit(PhoneDbContext context)
    {
        _context = context;
    }

    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyId { get; set; }

    [BindProperty()] public bool Value { get; set; } = false;

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (PropertyId is null)
        {
            throw new InvalidOperationException();
        }

        var propertyId = PropertyId.Value;

        var property = await _context.BooleanProperties
            .Include(item => item.BooleanPropertyType)
            .Include(item => item.Section)
            .ThenInclude(item => item.SectionType)
            .Include(item => item.Section)
            .ThenInclude(section => section.Phone)
            .Where(item => item.Id.Equals(propertyId)).ToListAsync().FirstOrNull();
        if (property is null)
        {
            ModelState.AddModelError(nameof(PropertyId), "property is not in database of boolean properties.");
        }

        BooleanProperty = property;
    }

    public BooleanProperty? BooleanProperty { get; set; } = null;

    public async Task<IActionResult> OnGet()
    {
        await ValidateRequest();
        if (BooleanProperty is null)
        {
            throw new InvalidOperationException();
        }

        Value = BooleanProperty.BoolData;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (BooleanProperty is null)
        {
            throw new InvalidOperationException();
        }


        BooleanProperty.BoolData = Value;

        return await PropertyPageModelUtilties.SaveAndRedirectIfSucceses(
            this,
            _context,
            ModelState,
            BooleanProperty.Section.PhoneId
        );
    }
}