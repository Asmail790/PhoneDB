using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.String;

public class Edit(PhoneDbContext context) : EditPropertyType<StringPropertyType>(context)
{
    protected override Task<StringPropertyType?> FindPropertyType(int propertyTypeId)
    {
        return Context.StringPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }
}