using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.SectionType;

public class Edit : PageModel
{
    private readonly DataBase.PhoneDbContext _context;
    [Required, BindProperty] public string? Name { get; set; }

    [Required, BindProperty] public string? Description { get; set; }

    [Required, BindProperty(SupportsGet = true)]
    public int? SectionTypeId { get; set; }

    public Edit(DataBase.PhoneDbContext context)
    {
        _context = context;
    }

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        SectionType = await _context.SectionTypes.FindAsync(SectionTypeId);
        if (SectionType is null)
        {
            ModelState.AddModelError(nameof(Edit.SectionType), "SectionId not in database");
        }
    }

    [BindProperty] public DataBase.SectionType? SectionType { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (SectionType is null)
        {
            throw new Exception();
        }


        Name = SectionType.Name;
        Description = SectionType.Description;


        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (SectionType is null)
        {
            throw new Exception();
        }


        if (Name is null)
        {
            throw new Exception();
        }

        SectionType.Name = Name;
        SectionType.Description = string.IsNullOrEmpty(Description) ? null : Description;

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("./Edit",
            new
            {
                SectionTypeId = SectionType.Id
            }));
    }
}