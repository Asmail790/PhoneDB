using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PhoneImage;

public class Delete : PageModel
{
    [BindProperty(SupportsGet = true), Required] public int PhoneImageId { get; set; }

    public DataBase.PhoneImage? PhoneImage{ get; set; }

    // Assuming you have a service to handle database operations and file deletions
    private readonly PhoneDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public Delete(PhoneDbContext phoneImageService, IWebHostEnvironment webHostEnvironment)
    {
        _context = phoneImageService;
        _webHostEnvironment = webHostEnvironment;

    }
    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        var errorMessage = "The image does not exist";
        var phoneImage = await _context.PhoneImages
        .Include(x => x.Phone)
        .Where(image => image.Id.Equals(PhoneImageId)).ToListAsync().FirstOrNull();
        if (phoneImage == null)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            return;
        }

        string imageName = phoneImage.Name;

        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
        var fileNotFound = !System.IO.File.Exists(imagePath);

        if (fileNotFound)
        {
            ModelState.AddModelError(string.Empty, errorMessage);
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
        
        string imageName = PhoneImage.Name;
        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);

        _context.PhoneImages.Remove(PhoneImage);

        try
        {
            System.IO.File.Delete(imagePath);

        }
        catch (IOException e)
        {
            // maybe also log out error to file
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the image.");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return await PhoneDB.Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("/Admin/Phone/Edit", new { phoneId = PhoneImage.PhoneId }));

    }
}