using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.Phone.Section.Property.Shared;
using PhoneDB.Pages.Admin.Property.Shared;

namespace PhoneDB.Pages.Admin.Property.Date;

public class Delete : PropertyRemover<DataBase.DateProperty>
{
    private PhoneDbContext _context;

    private Func<Task> _validateRequest;


    public Delete(PhoneDbContext context)
    {
        _context = context;
        _validateRequest = global::PhoneDB.Pages.Admin.Property.Shared.PropertyUtils.CreatePropertyDeleteRequestValidator(
            this,
            (propertyId) =>
                _context.DateProperties
                    .Include(item => item.DatePropertyType)
                    .Include(item => item.Section)
                    .ThenInclude(item => item.SectionType)
                    .Include(item => item.Section)
                    .ThenInclude(section => section.Phone)
                    .Where(item => item.Id.Equals(propertyId)).ToListAsync().FirstOrNull()
        );
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await _validateRequest();
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

        _context.DateProperties.Remove(Property);

        return await global::PhoneDB.Utils.Utils.SaveAndRedirectIfSuccessful(
            this,
            _context,
            ModelState,
            PropertyUtils.RedirectToPhoneEdit(this,Property.Section.PhoneId)
        );
    }
}