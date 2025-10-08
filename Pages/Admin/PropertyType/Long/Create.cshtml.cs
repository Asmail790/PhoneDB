using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Long;

public class Create : CreatePropertyType<LongPropertyType >
{
    public Create(PhoneDbContext context) : base(context)
    {
    }

    protected override LongPropertyType CreatePropertyTypeInstance(string name, string? description,
        DataBase.SectionType? sectionType)
    {
        return new LongPropertyType()
        {
            Name = name,
            Description = description,
            SectionType = sectionType
        };
    }

    protected override void AddPropertyTypeToDbContext(LongPropertyType propertyType)
    {
        Context.LongPropertyTypes.Add(propertyType);
    }

    protected override Task<bool> PropertyTypeExist(string name)
    {
        return Context.BooleanPropertyTypes.AnyAsync(item => item.Name.Equals(name));
    }
}