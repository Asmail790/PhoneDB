using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PhoneDB.Pages.Admin.PropertyType.Boolean;

namespace PhoneDB.Pages.Admin.PropertyType.Shared;

public abstract class EditPropertyType<T> : PageModel
    where T:DataBase.PropertyType
{
    [Required, BindProperty(SupportsGet = true)] public int? PropertyTypeId { get; set; }

    [Required, BindProperty] public string? Name { get; set; }
    [BindProperty] public string? Description { get; set; }
    public T? PropertyType { get; set; }

    protected readonly PhoneDbContext Context;

    protected abstract Task<T?> FindPropertyType(int id);

    public EditPropertyType(PhoneDbContext context)
    {
        Context = context;
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

        T? propertyType = await FindPropertyType(propertyTypeId);
        /*await
            Context.BooleanPropertyTypes
                .Include(item => item.SectionType)
                .Where(item => item.Id.Equals(propertyTypeId))
                .ToListAsync().FirstOrNull();
                */

        if (propertyType is null)
        {
            ModelState.AddModelError(nameof(EditPropertyType<T>.PropertyTypeId), "The propertyType id is not in db");
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
        DataBase.PropertyType propertyType = PropertyType;

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

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, Context, ModelState, RedirectToPage(
            "/Admin/SectionType/Edit", new
            {
                sectionTypeId = propertyType.SectionTypeId
            }
        ));
    }
}