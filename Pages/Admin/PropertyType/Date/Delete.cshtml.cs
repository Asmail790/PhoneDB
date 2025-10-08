using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Date;

public class Delete(PhoneDbContext context) : DeletePropertyType<DatePropertyType>(context)
{
    protected override async Task<DatePropertyType?> FindPropertyType(int propertyTypeId)
    {
        return await Context.DatePropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }

    protected override void RemovePropertyType(DatePropertyType propertyType)
    {
        Context.DatePropertyTypes.Remove(propertyType);
    }
}