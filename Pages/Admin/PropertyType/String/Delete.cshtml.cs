using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.String;

public class Delete(PhoneDbContext context) : DeletePropertyType<StringPropertyType>(context)
{
    protected override async Task<StringPropertyType?> FindPropertyType(int propertyTypeId)
    {
        return await Context.StringPropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }

    protected override void RemovePropertyType(StringPropertyType propertyType)
    {
        Context.StringPropertyTypes.Remove(propertyType);
    }
}