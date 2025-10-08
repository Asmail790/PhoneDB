using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.PropertyType.Shared;

namespace PhoneDB.Pages.Admin.PropertyType.String;

public class Create : CreatePropertyType<StringPropertyType >
{
    public Create(PhoneDbContext context) : base(context)
    {
    }

    protected override StringPropertyType CreatePropertyTypeInstance(string name, string? description,
        DataBase.SectionType? sectionType)
    {
        return new StringPropertyType()
        {
            Name = name,
            Description = description,
            SectionType = sectionType
        };
    }

    protected override void AddPropertyTypeToDbContext(StringPropertyType propertyType)
    {
        Context.StringPropertyTypes.Add(propertyType);
    }

    protected override Task<bool> PropertyTypeExist(string name)
    {
        return Context.StringPropertyTypes.AnyAsync(item => item.Name.Equals(name));
    }
}