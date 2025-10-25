using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace PhoneDB.Pages.Admin.PhoneImage;

public class Create : PageModel
{
    [BindProperty(SupportsGet = true), Required] public int PhoneId { get; set; }


    [BindProperty, Required,MaxLength(5),MinLength(1)] public List<IFormFile> Images { get; set; } = new();
    public DataBase.Phone? Phone { get; set; }

    private readonly PhoneDbContext _context;
    private readonly IWebHostEnvironment _env;

    public Create(PhoneDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    private async Task ValidateRequest()
    {
        var phone = await _context.Phones.Where(phone => phone.Id == PhoneId).ToListAsync().FirstOrNull();

        if (phone is null)
        {
            ModelState.AddModelError(nameof(PhoneId), $"Phone with id {PhoneId} does not exist.");
        }
        Phone = phone;
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
        

        var phone = Phone;

        if (phone is null)
        {
            throw new Exception();
        }
        
        var storedImages = _context.PhoneImages.Count(image => image.PhoneId == phone.Id);
        var totalImages = storedImages + Images.Count;

        var overLimit = Utils.TotalImagesPerPhone < totalImages;
        if (overLimit)
        {
            
            ModelState.AddModelError(string.Empty, $"""
            Total images limit {Utils.TotalImagesPerPhone} per phone is exceeded: {storedImages} stored images and {Images.Count} currently uploaded images.
            """);
            return Page();
        }



        var check = Images.Select(item => new
        {
            notValid = string.IsNullOrWhiteSpace(Path.GetExtension(item.FileName)),
            name = item.FileName
        }).ToArray();

        var someNotValid = check.Any(item => item.notValid);
        if (someNotValid)
        {
            foreach (var item in check.Where(item => item.notValid))
            {
                ModelState.AddModelError(item.name, $"The file named {item.name} do not have extension.");
            }
            return Page();
        }

        // TODO wait on all simultaneously. 
        foreach (var image in Images)
        {
            var uuid = Guid.NewGuid();

            var fileExtension = Path.GetExtension(image.FileName);
            var fileName = string.Join("",uuid.ToString(), fileExtension);
            var filePath = Path.Combine(_env.WebRootPath, "images", fileName);
            Console.WriteLine($"file will be saved at {filePath}");
            var phoneImage = new DataBase.PhoneImage()
            {
                Name = fileName,
                Phone = phone
            };

            _context.PhoneImages.Add(phoneImage);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await image.CopyToAsync(stream);
            Console.WriteLine($"file saved");
        }

        return await PhoneDB.Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("/Admin/Phone/Edit", new { phoneId = phone.Id }));

    }
}