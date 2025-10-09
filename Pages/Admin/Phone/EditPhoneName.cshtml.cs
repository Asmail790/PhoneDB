using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.Admin.Phone;

public class EditPhoneName : PageModel
{
    private readonly DataBase.PhoneDbContext _context;
    [Required, BindProperty] public string? Name { get; set; }
    [Required, BindProperty(SupportsGet = true)]  public int? PhoneId { get; set; }
    [BindProperty] public DataBase.Phone? Phone { get; set; }

    public EditPhoneName(DataBase.PhoneDbContext context)
    {
        _context = context;
    }

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        Phone = await _context.Phones.FindAsync(PhoneId);
        if (Phone is null)
        {
            ModelState.AddModelError(nameof(Admin.SectionType.Edit.SectionType), "PhoneId not in database");
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Phone is null)
        {
            throw new Exception();
        }

        Name = Phone.Name;


        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Phone is null)
        {
            throw new Exception();
        }


        if (Name is null)
        {
            throw new Exception();
        }

        Phone.Name = Name;

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("./Edit",
            new
            {
                PhoneId = Phone.Id
            }));
    }
}