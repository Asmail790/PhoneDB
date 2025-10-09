using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.Phone.Section.Property.Shared;
using PhoneDB.Pages.Admin.Property.Shared;
using PhoneDB.Utils;

namespace DataBase;

/**
 * research which approach is best: entity core inheritance for PropertyType or flat?
 *
 * table-per-hierarchy
 */
[Index(nameof(Name), nameof(SectionTypeId), IsUnique = true)]
public abstract class PropertyType
{
    public int Id { get; set; }
    public SectionType? SectionType { get; set; }
    public required  string Name { get; set; }


    private string? _description;

    public string? Description
    {
        get => _description;
        set => _description = string.IsNullOrWhiteSpace(value) ? null : value;
    }
    public abstract string TypeName { get; }
    public int SectionTypeId { get; set; }
    
    [NotMapped] public abstract PropertyType AsPropertyType { get; }
}

public class LongPropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "Long";
}

public class DoublePropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "Double";
}

public class DatePropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "Date";
}

public class BooleanPropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "Boolean";
}

public class StringPropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "String";
}

public abstract class Property
{
    public  required Section Section { get; set; }
    public int SectionId { get; set; }
    public int Id { get; set; }
    public abstract Object Data { get; }
    public abstract Property AsProperty { get; }
    public abstract PropertyType PropertyType { get; }
}

public class StringProperty : Property
{
    public required StringPropertyType StringPropertyType { get; set; }
    public int StringPropertyTypeId { get; set; }
    [NotMapped] public override Property AsProperty => this;
    public required string StringData { get; set; }
    [NotMapped] public override Object Data => StringData;
    public override PropertyType PropertyType => StringPropertyType;
}

public class LongProperty : Property
{
     public required  LongPropertyType LongPropertyType { get; set; }
    [NotMapped] public override Property AsProperty => this;
    public int LongPropertyTypeId { get; set; }
    public required long LongData { get; set; }
    [NotMapped] public override Object Data => LongData;
    public override PropertyType PropertyType => LongPropertyType;
}

public class DoubleProperty : Property
{
    public required DoublePropertyType DoublePropertyType { get; set; }
    [NotMapped] public override Property AsProperty => this;
    public int DoublePropertyTypeId { get; set; }
    public required double DoubleData { get; set; }
    [NotMapped] public override Object Data => DoubleData;
    public override PropertyType PropertyType => DoublePropertyType;
}

public class DateProperty : Property
{
    public required DatePropertyType DatePropertyType { get; set; }
    [NotMapped] public override Property AsProperty => this;
    public int DatePropertyTypeId { get; set; }
    public required DateTimeOffset DateTimeOffsetData { get; set; }
    [NotMapped] public override Object Data => DateTimeOffsetData;
    public override PropertyType PropertyType => DatePropertyType;
}

public class BooleanProperty : Property
{
    public required BooleanPropertyType BooleanPropertyType { get; set; }
    [NotMapped] public override Property AsProperty => this;
    public int BooleanPropertyTypeId { get; set; }
    public required bool BoolData { get; set; }
    [NotMapped] public override Object Data => BoolData;
    public override PropertyType PropertyType => BooleanPropertyType;
}

[Index(nameof(Name), IsUnique = true)]
public class SectionType
{
    public required string Name { get; set; }
    private string? _description;
    public string? Description
    {
        get => _description;
        set => _description = string.IsNullOrWhiteSpace(value) ? null : value;
    }
    public List<StringPropertyType> StringPropertyTypes { get; set; } = new List<StringPropertyType>();
    public List<LongPropertyType> LongPropertyTypes { get; set; } = new List<LongPropertyType>();
    public List<DoublePropertyType> DoublePropertyTypes { get; set; } = new List<DoublePropertyType>();
    public List<DatePropertyType> DatePropertyTypes { get; set; } = new List<DatePropertyType>();
    public List<BooleanPropertyType> BooleanPropertyTypes { get; set; } = new List<BooleanPropertyType>();
    public int Id { get; set; }
}

public class Section
{
    public List<StringProperty> StringProperties { get; set; } = new List<StringProperty>();
    public List<LongProperty> LongProperties { get; set; } = new List<LongProperty>();
    public List<DoubleProperty> DoubleProperties { get; set; } = new List<DoubleProperty>();
    public List<DateProperty> DateProperties { get; set; } = new List<DateProperty>();
    public List<BooleanProperty> BooleanProperties { get; set; } = new List<BooleanProperty>();

    public int Id { get; set; }
    public SectionType SectionType { get; set; }
    public int SectionTypeId { get; set; }
    public int PhoneId { get; set; }
    public required Phone Phone { get; set; }
}

[Index(nameof(Name), IsUnique = true)]
public class Phone
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Section> Sections { get; set; } = new();

    public List<PhoneReview> Reviews { get; set; } = new();
    public List<PhoneImageURL> Images { get; set; } = new();
}

public class PhoneReview
{
    public int User { get; set; }
    public int Id { get; set; }
    public int PhoneId { get; set; }
    public required Phone Phone { get; set; }

    [Range(1, 5, ErrorMessage = "Score must be between 1 and 5.")]
    public int Score { get; set; }

    public required string Description { get; set; }

    public required string Title { get; set; }

     public DateTimeOffset CreateDate { get; set; } 
    
}

public class PhoneImageURL
{
    public int Id { get; set; }
    public required string URL { get; set; }

    public int PhoneId { get; set; }
    public required Phone Phone { get; set; }
}

public class Image
{
    public int Id { get; set; }
    public required byte[]  Data { get; set; }
}