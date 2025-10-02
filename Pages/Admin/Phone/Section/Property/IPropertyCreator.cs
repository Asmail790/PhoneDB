namespace PhoneDB.Pages.Section.Property;

public interface IPropertyCreator<T0>
{
    public int? SectionId { get; set; }

    public int? PropertyTypeId { get; set; }

    public T0 Value { get; set; }

    public string? PhoneModel { get; set; }
    public string? SectionTypeName { get; set; }
    public string? PropertyTypeName { get; set; }
}