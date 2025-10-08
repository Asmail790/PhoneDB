using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.Date;

public class Create : CreatePropertyType<DatePropertyType >
{
    public Create(PhoneDbContext context) : base(context)
    {
    }

    protected override DatePropertyType CreatePropertyTypeInstance(string name, string? description,
        DataBase.SectionType? sectionType)
    {
        return new DatePropertyType()
        {
            Name = name,
            Description = description,
            SectionType = sectionType
        };
    }

    protected override void AddPropertyTypeToDbContext(DatePropertyType propertyType)
    {
        Context.DatePropertyTypes.Add(propertyType);
    }

    protected override Task<bool> PropertyTypeExist(string name)
    {
        return Context.BooleanPropertyTypes.AnyAsync(item => item.Name.Equals(name));
    }
}