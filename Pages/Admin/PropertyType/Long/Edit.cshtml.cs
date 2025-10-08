using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Long;

public class Edit(PhoneDbContext context) : EditPropertyType<LongPropertyType>(context)
{
    protected override Task<LongPropertyType?> FindPropertyType(int propertyTypeId)
    {
        return Context.LongPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }
}