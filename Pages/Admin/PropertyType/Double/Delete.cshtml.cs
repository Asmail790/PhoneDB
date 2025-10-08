using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Double;

public class Delete(PhoneDbContext context) : DeletePropertyType<DoublePropertyType>(context)
{
    protected override async Task<DoublePropertyType?> FindPropertyType(int propertyTypeId)
    {
        return await Context.DoublePropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }

    protected override void RemovePropertyType(DoublePropertyType propertyType)
    {
        Context.DoublePropertyTypes.Remove(propertyType);
    }
}