using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PropertyType.Shared;

public abstract class CreatePropertyType<T> : PageModel
    where T : DataBase.PropertyType
{
    public T? PropertyType { get; set; }
    public DataBase.SectionType? SectionType { get; set; }

    [Required(), BindProperty(SupportsGet = true)]
    public int? SectionTypeId { get; set; }

    [Required(), BindProperty()] public string? Name { get; set; }
    [BindProperty()] public string? Description { get; set; }

    protected readonly PhoneDbContext Context;

    protected abstract T CreatePropertyTypeInstance(
        string name,
        string? description,
        DataBase.SectionType? sectionType
    );

    protected abstract void AddPropertyTypeToDbContext(T propertyType);
    protected abstract Task<bool> PropertyTypeExist(string name);

    public CreatePropertyType(PhoneDbContext context)
    {
        Context = context;
    }

    private async Task ValidateSectionTypeRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (SectionTypeId is null)
        {
            throw new InvalidOperationException();
        }

        int sectionTypeId = SectionTypeId.Value;


        var sectionType = await Context.SectionTypes
            .Where(item => item.Id.Equals(sectionTypeId)).ToListAsync()
            .FirstOrNull();


        if (sectionType is null)
        {
            ModelState.AddModelError(nameof(CreatePropertyType<T>.SectionTypeId),
                $"A The section defined by id {sectionTypeId} don't exist.");
            return;
        }

        SectionType = sectionType;
    }

    private async Task ValidatePropertyType()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (Name is null)
        {
            throw new InvalidOperationException();
        }

        string name = Name;
        string? description = Description;

        var propertyTypeExist = await PropertyTypeExist(name);

        if (propertyTypeExist)
        {
            ModelState.AddModelError(nameof(CreatePropertyType<T>.Name), $"A  property with name {name} exist.");
            return;
        }

        if (SectionType is null)
        {
            throw new InvalidOperationException();
        }

        var sectionType = SectionType;


        PropertyType = CreatePropertyTypeInstance
        (
            name,
            description,
            sectionType
        );
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateSectionTypeRequest();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateSectionTypeRequest();
        await ValidatePropertyType();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PropertyType is null)
        {
            throw new InvalidOperationException();
        }

        if (SectionType is null)
        {
            throw new InvalidOperationException();
        }

        AddPropertyTypeToDbContext(PropertyType);

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, Context, ModelState, RedirectToPage(
            "/Admin/SectionType/Edit", new
            {
                sectionTypeId = SectionType.Id
            }));
    }
}