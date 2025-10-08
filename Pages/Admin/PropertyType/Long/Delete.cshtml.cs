using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Long;

public class Delete(PhoneDbContext context) : DeletePropertyType<LongPropertyType>(context)
{
    protected override async Task<LongPropertyType?> FindPropertyType(int propertyTypeId)
    {
        return await Context.LongPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }

    protected override void RemovePropertyType(LongPropertyType propertyType)
    {
        Context.LongPropertyTypes.Remove(propertyType);
    }
}