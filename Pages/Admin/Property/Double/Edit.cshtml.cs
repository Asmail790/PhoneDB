using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.Phone.Section.Property.Shared;
using PhoneDB.Pages.Admin.Property.Shared;

namespace PhoneDB.Pages.Admin.Property.Double;

public class Edit : PropertyEditor<DoubleProperty, double>
{
    private PhoneDbContext _context;
    private Func<Task> _validateRequest;

    public Edit(PhoneDbContext context)
    {
        _context = context;
        _validateRequest = PropertyUtils.CreatePropertyEditRequestValidator(
            this,
            propertyId => _context.DoubleProperties
                .Include(item => item.DoublePropertyType)
                .Include(item => item.Section)
                .ThenInclude(item => item.SectionType)
                .Include(item => item.Section)
                .ThenInclude(section => section.Phone)
                .Where(item => item.Id.Equals(propertyId)).ToListAsync().FirstOrNull()
        );
    }
    
    public async Task<IActionResult> OnGet()
    {
        await _validateRequest();
        if (Property is null)
        {
            throw new InvalidOperationException();
        }

        Value = Property.DoubleData;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _validateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Property is null)
        {
            throw new InvalidOperationException();
        }


        Property.DoubleData = Value;

        return await Utils.Utils.SaveAndRedirectIfSuccessful(
            this,
            _context,
            ModelState,
            PropertyUtils.RedirectToPhoneEdit(this,Property.Section.PhoneId)
        );
    }
}