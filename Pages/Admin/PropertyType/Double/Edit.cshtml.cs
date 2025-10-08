using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Double;

public class Edit(PhoneDbContext context) : EditPropertyType<DoublePropertyType>(context)
{
    protected override Task<DoublePropertyType?> FindPropertyType(int propertyTypeId)
    {
        return Context.DoublePropertyTypes
            .Include(item => item.SectionType)
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync().FirstOrNull();
    }
}