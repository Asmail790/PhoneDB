using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Date;

public class Edit(PhoneDbContext context) : EditPropertyType<DatePropertyType>(context)
{
    protected override Task<DatePropertyType?> FindPropertyType(int propertyTypeId)
    {
        return Context.DatePropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }
}