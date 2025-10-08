using DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Boolean;

public class Create : CreatePropertyType<BooleanPropertyType>
{
    public Create(PhoneDbContext context) : base(context)
    {
    }

    protected override BooleanPropertyType CreatePropertyTypeInstance(string name, string? description,
        DataBase.SectionType? sectionType)
    {
        return new BooleanPropertyType()
        {
            Name = name,
            Description = description,
            SectionType = sectionType
        };
    }

    protected override void AddPropertyTypeToDbContext(BooleanPropertyType propertyType)
    {
        Context.BooleanPropertyTypes.Add(propertyType);
    }

    protected override Task<bool> PropertyTypeExist(string name)
    {
        return Context.BooleanPropertyTypes.AnyAsync(item => item.Name.Equals(name));
    }
}