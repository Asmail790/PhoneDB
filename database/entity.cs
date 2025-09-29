using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
    public string? Description { get; set; }
    public abstract string TypeName { get; }

    public SectionType SectionType { get; set; }
    public int SectionTypeId { get; set; }

    public string Name { get; set; }

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
    public override string TypeName => "Bool";
}

public class StringPropertyType : PropertyType
{
    [NotMapped] public override PropertyType AsPropertyType => this;
    public override string TypeName => "String";
}

public abstract class Property
{
    public required Section Section { get; set; }
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
    public required LongPropertyType LongPropertyType { get; set; }
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

[Index(nameof(Name),IsUnique = true)]
public class SectionType
{
    public string Name { get; set; }
    public string? Description { get; set; }
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

[Index(nameof(PhoneModel), IsUnique = true)]
public class Phone
{
    public int Id { get; set; }
    public required string PhoneModel { get; set; }
    public List<Section> Sections { get; set; } = new();

    public List<PhoneReview> Reviews { get; set; } = new();
    // public required  <PhoneImage> Images { get; set; }
    // public required PhoneImage thumbnail { get; set; }
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
}

public class PhoneImage
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }

    public int PhoneId { get; set; }
    public required Phone Phone { get; set; }
}

public static class Populater
{
    public static SectionType GeneralSectionKey = new()
        { Description = "General information about the device", Name = "General" };

    public static SectionType NetworkSectionType = new()
        { Description = "Network info", Name = "Network" };

    public static SectionType CameraSectionType = new() { Name = "Camera", Description = "Camera related information" };

    public static SectionType DisplaySectionType = new()
        { Name = "Display", Description = "Display related information" };

    public static SectionType BatterySectionType = new()
        { Name = "Battery", Description = "Battery related information" };

    public static SectionType PerformanceSectionTpe =
        new() { Name = "Performance", Description = "Performance related information" };

    public static StringPropertyType BrandPropType = new()
    {
        Name = "Brand", Description = "Brand name of the device. This may match with the manufacturer",
        SectionType = GeneralSectionKey
    };

    public static DatePropertyType ReleasedPropType = new()
    {
        Name = "Released", Description = "First official market release date in (year, month)",
        SectionType = GeneralSectionKey
    };

    public static StringPropertyType OsPropType = new()
    {
        Name = "OS", Description = "Operating System of the device. This may include the version number too",
        SectionType = GeneralSectionKey
    };


    public static StringPropertyType ChipsetPropType = new()
    {
        Name = "Chipset", Description = "Chipset used in the device. This may include the version number too",
        SectionType = GeneralSectionKey
    };

    public static StringPropertyType IPCodeType = new()
    {
        Name = "IP code",
        Description =
            "The IP code or Ingress Protection code indicates how well a device is protected against water and dust. ",
        SectionType = GeneralSectionKey
    };

    public static BooleanPropertyType FoldableType = new()
    {
        Name = "Foldable Phone",
        Description =
            "The IP code or Ingress Protection code indicates how well a device is protected against water and dust. ",
        SectionType = GeneralSectionKey
    };

    private static async Task AddUnusedSectionTypes(ApplicationDbContext context)
    {
        context.SectionTypes.Add(NetworkSectionType);
        await context.SaveChangesAsync();
    }

    private static async Task AddUnusedPropertyTypes(ApplicationDbContext context)
    {
        context.StringPropertyTypes.Add(IPCodeType);
        context.BooleanPropertyTypes.Add(FoldableType);
        await context.SaveChangesAsync();
    }


    private static Phone CreateSamsung(ApplicationDbContext context, int id)
    {
        var phone = new Phone()
        {
            PhoneModel = $"Galaxy S21-{id}",
        };
        var generalSection = new Section
        {
            Phone = phone,
            SectionType = GeneralSectionKey
        };

        var brandProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = BrandPropType,
            StringData = "Samsung",
        };

        var releasedProperty = new DateProperty()
        {
            Section = generalSection,
            DatePropertyType = ReleasedPropType,
            DateTimeOffsetData = new DateTimeOffset(new DateTime(2024, 10, 2))
        };

        var osProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = OsPropType,
            StringData = "Android 11"
        };

        var chipsetProperty = new StringProperty
        {
            Section = generalSection,
            StringPropertyType = ChipsetPropType,
            StringData = "Exynos 2100"
        };


        var displaySection = new Section { SectionType = DisplaySectionType, Phone = phone };
        var cameraSection = new Section { SectionType = CameraSectionType, Phone = phone };
        var batterySection = new Section { SectionType = BatterySectionType, Phone = phone };
        var performanceSection = new Section { SectionType = PerformanceSectionTpe, Phone = phone };

        generalSection.StringProperties.AddRange(
            brandProperty, osProperty, chipsetProperty
        );
        generalSection.DateProperties.Add(releasedProperty);

        phone.Sections.AddRange(
            generalSection,
            displaySection,
            cameraSection,
            batterySection,
            performanceSection
        );


        context.Phones.Add(phone);
        return phone;
    }

    private static Phone CreateApple(ApplicationDbContext context, int id)
    {
        var phone = new Phone()
        {
            PhoneModel = $"iPhone 11-{id}"
        };

        var generalSection = new Section
        {
            SectionType = GeneralSectionKey,
            Phone = phone
        };
        var brandProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = BrandPropType,
            StringData = "Apple"
        };


        var releasedProperty = new DateProperty()
        {
            Section = generalSection,
            DatePropertyType = ReleasedPropType,
            DateTimeOffsetData = new DateTimeOffset(new DateTime(2019, 10, 2))
        };

        var osProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = OsPropType,
            StringData = "Apple iOS 13"
        };

        var chipsetProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = ChipsetPropType,
            StringData = "Apple A13 Bionic"
        };


        var displaySection = new Section { SectionType = DisplaySectionType, Phone = phone };
        var cameraSection = new Section { SectionType = CameraSectionType, Phone = phone };
        var batterySection = new Section { SectionType = BatterySectionType, Phone = phone };
        var performanceSection = new Section { SectionType = PerformanceSectionTpe, Phone = phone };

        phone.Sections.AddRange(
            generalSection,
            displaySection,
            cameraSection,
            batterySection,
            performanceSection);

        generalSection.StringProperties.AddRange(
            brandProperty, osProperty, chipsetProperty
        );
        generalSection.DateProperties.Add(releasedProperty);
        context.Phones.Add(phone);
        return phone;
    }

    private static Phone CreatePixel(ApplicationDbContext context, int id)
    {
        var phone = new Phone
        {
            PhoneModel = $"Pixel 9a-{id}",
        };

        var generalSection = new Section
        {
            SectionType = GeneralSectionKey,
            Phone = phone
        };

        var brandProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = BrandPropType,
            StringData = "Google"
        };


        var releasedProperty = new DateProperty()
        {
            Section = generalSection,
            DatePropertyType = ReleasedPropType,
            DateTimeOffsetData = new DateTimeOffset(new DateTime(2025, 6, 3))
        };

        var osProperty = new StringProperty()
        {
            Section = generalSection,
            StringPropertyType = OsPropType,
            StringData = "Google Android 15"
        };

        var chipsetProperty = new StringProperty
        {
            Section = generalSection,
            StringPropertyType = ChipsetPropType,
            StringData = "Samsung Google Tensor G4"
        };

        generalSection.StringProperties.AddRange(
            brandProperty, osProperty, chipsetProperty
        );
        generalSection.DateProperties.Add(releasedProperty);

        var displaySection = new Section { SectionType = DisplaySectionType, Phone = phone };
        var cameraSection = new Section { SectionType = CameraSectionType, Phone = phone };
        var batterySection = new Section { SectionType = BatterySectionType, Phone = phone };
        var performanceSection = new Section { SectionType = PerformanceSectionTpe, Phone = phone };

        generalSection.DateProperties.Add(releasedProperty);
        context.Phones.Add(phone);
        phone.Sections.AddRange(
            generalSection,
            displaySection,
            cameraSection,
            batterySection,
            performanceSection
        );
        context.Phones.Add(phone);

        return phone;
    }

    private static void CreateReview(this ApplicationDbContext context, Phone phone, int score, string description,
        string title, int userId)
    {
        var review = new PhoneReview
        {
            Phone = phone,
            User = userId,
            Description = description,
            Title = title,
            Score = score
        };

        context.PhoneReviews.Add(review);
    }

    public static async Task PopulateDb(this IServiceProvider provider)
    {
        await using var serviceContext = provider.CreateAsyncScope();
        var context = serviceContext.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await AddUnusedPropertyTypes(context);
        await AddUnusedSectionTypes(context);


        if (context.Phones.Count().Equals(0))
        {
            foreach (var i in Enumerable.Range(0, 30))
            {
                CreateApple(context, i);
                CreateSamsung(context, i);
                CreatePixel(context, i);
            }


            await context.SaveChangesAsync();
        }

        if (context.PhoneReviews.Count().Equals(0))
        {
            foreach (var phone in context.Phones)
            {
                /*CreateReview(context, phone, 3, "good", "better", 0);
                CreateReview(context, phone, 3, "good", "better", 1);
                CreateReview(context, phone, 3, "good", "better", 4);*/
            }

            await context.SaveChangesAsync();
        }
    }
}