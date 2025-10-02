namespace DataBase;

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

    private static async Task AddUnusedSectionTypes(PhoneDbContext context)
    {
        context.SectionTypes.Add(NetworkSectionType);
        await context.SaveChangesAsync();
    }

    private static async Task AddUnusedPropertyTypes(PhoneDbContext context)
    {
        context.StringPropertyTypes.Add(IPCodeType);
        context.BooleanPropertyTypes.Add(FoldableType);
        await context.SaveChangesAsync();
    }


    private static Phone CreateSamsung(PhoneDbContext context, int id)
    {
        var phone = new Phone()
        {
            Name = $"Galaxy S21-{id}",
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
            //generalSection,
            displaySection,
            cameraSection,
            batterySection,
            performanceSection
        );


        context.Phones.Add(phone);
        return phone;
    }

    private static Phone CreateApple(PhoneDbContext context, int id)
    {
        var phone = new Phone()
        {
            Name = $"iPhone 11-{id}"
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

    private static Phone CreatePixel(PhoneDbContext context, int id)
    {
        var phone = new Phone
        {
            Name = $"Pixel 9a-{id}",
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

    private static void CreateReview(this PhoneDbContext context, Phone phone, int score, string description,
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
        var context = serviceContext.ServiceProvider.GetRequiredService<PhoneDbContext>();

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