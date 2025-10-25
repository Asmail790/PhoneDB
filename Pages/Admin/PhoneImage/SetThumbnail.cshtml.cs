using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PhoneImage;

public class SetThumbnail : PageModel
{
    [BindProperty(SupportsGet = true), Required] public int PhoneImageId { get; set; }
    public DataBase.PhoneImage? PhoneImage { get; set; }


    private readonly PhoneDbContext _context;
    public SetThumbnail(PhoneDbContext context)
    {
        _context = context;

    }
    private async Task ValidateRequest()
    {

        if (!ModelState.IsValid)
        {
            return;
        }
        
        // Check if the phone image with PhoneImageId exists and is part of the phone
        var phoneImage = await _context.PhoneImages
            .Include(x => x.Phone)
            .Where(image => image.Id.Equals(PhoneImageId) && image.Id.Equals(PhoneImageId))
            .ToListAsync()
            .FirstOrNull();

        if (phoneImage == null)
        {
            ModelState.AddModelError(string.Empty, "The image does not exist");
            return;
        }
        PhoneImage = phoneImage;
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
        if (PhoneImage is null)
        {
            throw new Exception();
        }

        var phone = PhoneImage.Phone;
        phone.Thumbnail = PhoneImage;

        return await PhoneDB.Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("/Admin/Phone/Edit", new { phoneId = PhoneImage.PhoneId }));

    }
}