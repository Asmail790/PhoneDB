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
            Score = score,
            
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
                CreateReview(context, phone, 4, """
                                                I recently purchased this phone and I must say, it's been an absolute pleasure to use. The design is sleek and modern, and the build quality feels solid. The camera takes great photos, even in low light conditions, which is a huge plus for me.
                                                
                                                The battery life is fantastic too. It lasts all day without needing a recharge, and I love that it has fast charging capabilities. The phone runs smoothly, with quick app launches and a seamless user interface.
                                                
                                                Overall, this phone has exceeded my expectations. If you're looking for a reliable and feature-rich device, I highly recommend giving this one a try!
                                                
                                                """, "Great Phone Experience!", 0);
                CreateReview(context, phone, 5, """
                                                I've been using this phone for about a month now and I'm really impressed with its performance. The display is sharp and vibrant, making it easy on the eyes. The battery life is quite good too, lasting all day without needing to plug in.
                                                
                                                The phone is also very user-friendly, with intuitive controls that make navigation easy. It has great connectivity options, including a fast 5G network and Bluetooth for wireless accessories. The camera quality is decent, capturing clear photos and videos.
                                                
                                                What I like most about this phone is its durability. It's well-built and seems to withstand drops and bumps without any issues. The price point is also quite reasonable compared to similar devices on the market.
                                                
                                                Overall, I'm very satisfied with my purchase. If you're looking for a reliable and budget-friendly smartphone, this one definitely stands out!
                                                
                                                """, "Excellent Value for Money", 7);
                CreateReview(context, phone, 1, """
                                                So, I recently purchased the *UltimatePhone 12 Pro Max*—a device that promised to revolutionize my mobile experience. Let me tell you, it’s a 
                                                masterclass in how not to make a phone.  
                                                
                                                **Battery Life: A Disappointment**  
                                                The battery life is *so* short, it’s like the phone was designed by a toddler with a sugar rush. I used it for 6 hours straight (scrolling TikTok, 
                                                texting, and streaming a cat video) and it died mid-video. The “all-day battery” claim is a lie. Oh, and the charging port? It’s so flimsy, I 
                                                accidentally dropped the phone while charging and now it won’t charge at all.  
                                                
                                                **Performance: A Slow, Clunky Mess**  
                                                This phone feels like it’s from 2012. Apps lag, freeze, and crash constantly. Even basic tasks like opening the camera or switching between apps feel 
                                                like waiting for a slow elevator. The “high-end processor” is a joke—my old phone from 2018 runs smoother.  
                                                
                                                **Camera: A Disaster**  
                                                The camera? Let’s just say it’s not fit for a Instagram influencer. Photos are blurry, colors are off, and low-light shots are like looking through a 
                                                foggy window. I tried to take a sunset photo, and it came out looking like a cloudy day. The front camera is equally bad—selfies look like they were 
                                                taken with a flashlight.  
                                                
                                                **Build Quality: Cheap and Flimsy**  
                                                The phone feels like it’s made of cardboard and plastic. The frame is so cheap, I’m worried it’ll break if I accidentally drop it. The back panel is 
                                                glossy and scratches easily. And the buttons? They feel like they’re about to fall off.  
                                                
                                                **Software: A Clunky, Bug-Prone Mess**  
                                                The OS is a nightmare. It’s riddled with glitches, crashes, and weird UI bugs. The “smart assistant” is useless—it doesn’t understand basic commands. 
                                                And the updates? They’re so buggy, I’ve had to factory reset twice.  
                                                
                                                **Final Verdict: A Waste of Money**  
                                                If you’re looking for a phone that’s reliable, fast, and actually works, the *UltimatePhone 12 Pro Max* is not for you. It’s a scam disguised as a 
                                                gadget. Save your money and go for something that doesn’t feel like it’s trying to break your soul.  
                                                
                                                **Rating: 1/10**  
                                                *Why? Because even the stars are tired of this phone.* 
                                                
                                                """, "Phone Review: The \"Ultimate\" Experience (Spoiler: It’s Not)", 6);
                
                CreateReview(context, phone, 3, "good", "1 better", 1);
                
                CreateReview(context, phone, 3, "good", "4 better", 4);
            }

            await context.SaveChangesAsync();
        }
    }
}