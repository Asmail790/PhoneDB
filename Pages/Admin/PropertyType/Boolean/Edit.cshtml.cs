using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Boolean;

/*
public class Edit : PageModel
{
    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyTypeId { get; set; }

    [Required, BindProperty] public string? Name { get; set; }
    [BindProperty] public string? Description { get; set; }
    public BooleanPropertyType? PropertyType { get; set; }

    private readonly PhoneDbContext _context;

    public Edit(PhoneDbContext context)
    {
        _context = context;
    }

    public async Task ValidateRequest()
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

        BooleanPropertyType? propertyType = await
            _context.BooleanPropertyTypes
                .Include(item => item.SectionType)
                .Where(item => item.Id.Equals(propertyTypeId))
                .ToListAsync().FirstOrNull();

        if (propertyType is null)
        {
            ModelState.AddModelError(nameof(Edit.PropertyTypeId), "The propertyType id is not in db");
        }

        PropertyType = propertyType;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PropertyType is null)
        {
            throw new InvalidOperationException();
        }

        Name = PropertyType.Name;
        Description = PropertyType.Description;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PropertyType is null)
        {
            throw new InvalidOperationException();
        }

        if (Description is null)
        {
            throw new InvalidOperationException();
        }

        if (Name is null)
        {
            throw new InvalidOperationException();
        }

        PropertyType.Name = Name;
        PropertyType.Description = Description;

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage(
            "/Admin/SectionType/Edit", new
            {
                sectionTypeId = PropertyType.SectionType.Id
            }
        ));
    }
}
*/

public class Edit(PhoneDbContext context) : EditPropertyType<BooleanPropertyType>(context)
{
    protected override Task<BooleanPropertyType?> FindPropertyType(int propertyTypeId)
    {
        return Context.BooleanPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }
}