using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.Admin.PropertyType.Shared;

public abstract class DeletePropertyType<T>(PhoneDbContext context) : PageModel
    where T : DataBase.PropertyType
{
    protected readonly PhoneDbContext Context = context;

    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyTypeId { get; set; }

    public T? PropertyType { get; set; }

    protected abstract Task<T?> FindPropertyType(int id);
    protected abstract void RemovePropertyType(T type);

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

        var propertyType = await FindPropertyType(propertyTypeId);
        /*await _context.BooleanPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();*/


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
        T propertyType = PropertyType;

        RemovePropertyType(PropertyType);

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, Context, ModelState,
            RedirectToPage(
                "/Admin/SectionType/Edit", new
                {
                    sectionTypeId =propertyType.SectionTypeId
                }));
    }
}