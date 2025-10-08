using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Double;

public class Create : CreatePropertyType<DoublePropertyType >
{
    public Create(PhoneDbContext context) : base(context)
    {
    }

    protected override DoublePropertyType CreatePropertyTypeInstance(string name, string? description,
        DataBase.SectionType? sectionType)
    {
        return new DoublePropertyType()
        {
            Name = name,
            Description = description,
            SectionType = sectionType
        };
    }

    protected override void AddPropertyTypeToDbContext(DoublePropertyType propertyType)
    {
        Context.DoublePropertyTypes.Add(propertyType);
    }

    protected override Task<bool> PropertyTypeExist(string name)
    {
        return Context.BooleanPropertyTypes.AnyAsync(item => item.Name.Equals(name));
    }
}