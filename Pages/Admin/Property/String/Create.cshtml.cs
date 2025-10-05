using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.Phone.Section.Property.Shared;
using PhoneDB.Pages.Admin.Property.Shared;

namespace PhoneDB.Pages.Admin.Property.String;

public class Create : PropertyCreator<string, StringPropertyType>
{
    private PhoneDbContext _context;
    private Func<Task> _validateRequest;

    public Create(PhoneDbContext context)
    {
        _context = context;
        _validateRequest = PropertyUtils.CreatePropertyAddRequestValidator(
            this,
            _context,
            (propertyTypeId) =>
            {
                return context.StringPropertyTypes.Where(item => item.Id.Equals(propertyTypeId)).ToListAsync()
                    .FirstOrNull();
            },
            ((int propertyTypeId, int sectionId) args) =>
            {
                return context.Sections.AnyAsync(item =>
                    item.Id.Equals(args.sectionId) &&
                    item.SectionType.StringPropertyTypes.Any(type => type.Id.Equals(args.propertyTypeId)))
                    .ContinueWith(item =>
                    {
                        var included = item.Result;
                        return !included;
                    });
            },
            ((int propertyTypeId, int sectionId) args) =>
            {
                return context.StringProperties.AnyAsync(item =>
                    item.SectionId.Equals(args.sectionId) && item.StringPropertyTypeId.Equals(args.propertyTypeId));
            }
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

        if (Section is null || PropertyType is null)
        {
            throw new InvalidOperationException();
        }


        var item = new StringProperty()
        {
            Section = Section,
            StringData = Value,
            StringPropertyType = PropertyType
        };

        _context.StringProperties.Add(item);
        return await global::PhoneDB.Utils.Utils.SaveAndRedirectIfSuccessful(
            this,
            _context,
            ModelState,
            PropertyUtils.RedirectToPhoneEdit(this,Section.PhoneId)
        );
    }
}