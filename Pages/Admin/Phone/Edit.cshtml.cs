using System.Collections.Immutable;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace PhoneDB.Pages.Admin.Phone;

public class PropertyTypeComparer : IEqualityComparer<PropertyType>
{
    public bool Equals(PropertyType? x, PropertyType? y)
    {
        return x?.Id.Equals(y?.Id) ?? false;
    }

    public int GetHashCode(PropertyType obj)
    {
        return obj.Id;
    }
}

public class SectionComparer : IEqualityComparer<DataBase.Section>
{
    public bool Equals(DataBase.Section? x, DataBase.Section? y)
    {
        return x?.Id.Equals(y?.Id) ?? false;
    }

    public int GetHashCode(DataBase.Section obj)
    {
        return obj.Id;
    }
}

public class SectionTypeComparer : IEqualityComparer<DataBase.SectionType>
{
    public bool Equals(DataBase.SectionType? x, DataBase.SectionType? y)
    {
        return x?.Id.Equals(y?.Id) ?? false;
    }

    public int GetHashCode(DataBase.SectionType obj)
    {
        return obj.Id;
    }
}
public class EditModel : PageModel
{
    private readonly DataBase.PhoneDbContext _context;

    public EditModel(DataBase.PhoneDbContext context)
    {
        _context = context;
    }

    [BindProperty] public DataBase.Phone Phone { get; set; } = default!;

    public Dictionary<DataBase.Section, List<PropertyType>> UnusedPropertyTypesPerSection =
        new Dictionary<DataBase.Section, List<PropertyType>>(new SectionComparer());

    public Dictionary<DataBase.Section, List<DataBase.Property>> PropertiesPerSection =
        new Dictionary<DataBase.Section, List<DataBase.Property>>(new SectionComparer());

    public List < DataBase.SectionType> UnusedSectionTypes =  new ();


    private  static List<DataBase.SectionType> CalculateAllUnusedSections(List<DataBase.SectionType> allSectionTypes,DataBase.Phone phone)
    {
        var allSectionTypesSet = new HashSet<DataBase.SectionType>(allSectionTypes,new SectionTypeComparer());
        var allUsedSections = phone.Sections.Select(item => item.SectionType);
        return allSectionTypesSet.Except(allUsedSections).ToList();


    }
    private static Dictionary<DataBase.Section, List<PropertyType>> CalculateUnusedPropertyTypesPerSection(DataBase.Phone phone)
    {
       var unusedPropertyTypesPerSection = new Dictionary<DataBase.Section, List<PropertyType>>(new SectionComparer());
        phone.Sections.ForEach(section =>
        {
             var data = CalculateUnusedPropertyTypes(section);
             unusedPropertyTypesPerSection.Add(data.Key,data.Value);
        });
        
        return unusedPropertyTypesPerSection;
    }
    
    private static Dictionary<DataBase.Section, List<DataBase.Property>> CalculatePropertiesPerSection(DataBase.Phone phone)
    {
        var propertiesPerSection = new Dictionary<DataBase.Section, List<DataBase.Property>>(new SectionComparer());
        phone.Sections.ForEach(section =>
        {
            var allStringProperties = section.StringProperties.Select(item => item.AsProperty);
            var allDateProperties = section.DateProperties.Select(item => item.AsProperty);
            var allDoubleProperties = section.DoubleProperties.Select(item => item.AsProperty);
            var allLongProperties = section.LongProperties.Select(item => item.AsProperty);
            var allBooleanProperties = section.BooleanProperties.Select(item => item.AsProperty);

            var allProperties = new List<DataBase.Property>();
            
            allProperties.AddRange( allStringProperties); 
            allProperties.AddRange( allDateProperties); 
            allProperties.AddRange( allDoubleProperties); 
            allProperties.AddRange( allLongProperties); 
            allProperties.AddRange( allBooleanProperties);
            

            var pair = new KeyValuePair<DataBase.Section, List<DataBase.Property>>(section, allProperties); 
          propertiesPerSection.Add(pair.Key,pair.Value);
        });
        
        return propertiesPerSection;
    } 
    private static KeyValuePair<DataBase.Section, List<PropertyType>> CalculateUnusedPropertyTypes(DataBase.Section section)
    {
        var allStringPropertyTypes = section.SectionType.StringPropertyTypes.Select(item => item);
        var allDatePropertyTypes = section.SectionType.DatePropertyTypes.Select(item => item);
        var allDoublePropertyTypes = section.SectionType.DoublePropertyTypes.Select(item => item);
        var allLongPropertyTypes = section.SectionType.LongPropertyTypes.Select(item => item);
        var allBooleanPropertyTypes = section.SectionType.BooleanPropertyTypes.Select(item => item);


        var usedStringPropertyTypes = section.StringProperties.Select(item => item.StringPropertyType);
        var usedDatePropertyTypes = section.DateProperties.Select(item => item.DatePropertyType);
        var usedDoublePropertyTypes = section.DoubleProperties.Select(item => item.DoublePropertyType);
        var usedLongPropertyTypes = section.LongProperties.Select(item => item.LongPropertyType);
        var usedBooleanPropertyTypes = section.BooleanProperties.Select(item => item.BooleanPropertyType);

        var setOfStringPropertyTypes = ImmutableHashSet.CreateRange<StringPropertyType>(new PropertyTypeComparer(),allStringPropertyTypes)
            .Except(usedStringPropertyTypes);
        var setOfDatePropertyTypes = ImmutableHashSet.CreateRange<DatePropertyType>(new PropertyTypeComparer(),allDatePropertyTypes)
            .Except(usedDatePropertyTypes);
        var setOfDoublePropertyTypes = ImmutableHashSet.CreateRange<DoublePropertyType>(new PropertyTypeComparer(),allDoublePropertyTypes)
            .Except(usedDoublePropertyTypes);
        var setOfLongPropertyTypes = ImmutableHashSet.CreateRange<LongPropertyType>(new PropertyTypeComparer(),allLongPropertyTypes)
            .Except(usedLongPropertyTypes);
        var setOfBooleanPropertyTypes = ImmutableHashSet.CreateRange<BooleanPropertyType>(new PropertyTypeComparer(),allBooleanPropertyTypes)
            .Except(usedBooleanPropertyTypes);
        var allUnusedProperties = new List<PropertyType>();
        
        allUnusedProperties.AddRange( setOfStringPropertyTypes); 
        allUnusedProperties.AddRange(setOfDatePropertyTypes);
        allUnusedProperties.AddRange(setOfDoublePropertyTypes);
        allUnusedProperties.AddRange(setOfLongPropertyTypes);
        allUnusedProperties.AddRange(setOfBooleanPropertyTypes);

        return new KeyValuePair<DataBase.Section, List<PropertyType>>(section, allUnusedProperties);
    }

    public async Task<IActionResult> OnGetAsync(int? phoneId)
    {
        
        if (phoneId == null)
        {
            return NotFound();
        }

        var phone = await this._context.Phones.Where(m => m.Id == phoneId)
            // StringPropertyTypes type for Section Types
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.SectionType)
            .ThenInclude(type => type.StringPropertyTypes)
            // BooleanPropertyTypes type for Section Types
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.SectionType)
            .ThenInclude(type => type.BooleanPropertyTypes)
            // LongPropertyTypes type for Section Types
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.SectionType)
            .ThenInclude(type => type.LongPropertyTypes)
            // DoublePropertyTypes type for Section Types
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.SectionType)
            .ThenInclude(type => type.DoublePropertyTypes)
            // DatePropertyTypes type for Section Types
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.SectionType)
            .ThenInclude(type => type.DatePropertyTypes)
            
            
            //BooleanPropertyType
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.BooleanProperties)
            .ThenInclude(properties => properties.BooleanPropertyType)

            //DoublePropertyType
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.DoubleProperties)
            .ThenInclude(properties => properties.DoublePropertyType)

            //LongPropertyType
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.LongProperties)
            .ThenInclude(properties => properties.LongPropertyType)

            //DatePropertyType
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.DateProperties)
            .ThenInclude(properties => properties.DatePropertyType)

            //StringPropertyType
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.StringProperties)
            .ThenInclude(properties => properties.StringPropertyType)
            .FirstOrDefaultAsync();


        if (phone == null)
        {
            return NotFound();
        }

        Phone = phone;
        UnusedPropertyTypesPerSection = CalculateUnusedPropertyTypesPerSection(phone);
        PropertiesPerSection = CalculatePropertiesPerSection(phone);
        UnusedSectionTypes = CalculateAllUnusedSections(await _context.SectionTypes.ToListAsync(),phone);

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Phone).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PhoneExists(Phone.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool PhoneExists(int id)
    {
        return _context.Phones.Any(e => e.Id == id);
    }
}