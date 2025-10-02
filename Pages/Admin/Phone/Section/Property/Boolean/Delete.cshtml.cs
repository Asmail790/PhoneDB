using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Section.Property;


public class Delete : PageModel
{
    private PhoneDbContext _context;

    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyId { get; set; }

    public BooleanProperty? Property { get; set; }

    public Delete(PhoneDbContext context)
    {
        _context = context;
    }
    
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
            ModelState.AddModelError(string.Empty, "Property with this id not found in boolean database.");
        }

        Property = property;
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

        if (Property is null)
        {
            throw new InvalidOperationException();
        }

        _context.BooleanProperties.Remove(Property);

        return await PropertyPageModelUtilties.SaveAndRedirectIfSucceses(
            this,
            _context,
            ModelState,
            Property.Section.PhoneId
        );
    }
}