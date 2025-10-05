using Extra;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DataBase;

public interface IPhoneDbServiceLoader
{
    // Load methods 
    // load all SectionsAndPropertiesForPhone
    // load all SectionTypes
    // load all propertyTypes
    // load all ImagesForPhone


    public Task LoadAllTypesIntoContext();

    public Task LoadAllBooleanPropertyTypesIntoContext();

    public Task LoadAllStringPropertyTypesIntoContext();

    public Task LoadAllDoublePropertyTypesIntoContext();

    public Task LoadAllDatePropertyTypesIntoContext();

    public Task LoadAllLongPropertyTypesIntoContext();

    public Task LoadAllSectionTypesIntoContext();

    public Task<bool> LoadPhonePropsIntoContext(int phoneId);
}

public static class ListHelperExtension
{
    public static T? FirstOrNull<T>(this List<T> list) where T : class
    {
        return list.Count.Equals(0) ? null : list[0];
    }

    public static T? FirstOrNull<T>(this List<T> list, Func<T, bool> lambda) where T : class
    {
        return list.Where(lambda).ToList().Count.Equals(0) ? null : list[0];
    }
}

public static class TaskExtension
{
    public static async Task<T?> FirstOrNull<T>(this Task<List<T>> task) where T : class
    {
        var list = await task;
        return list.Count.Equals(0) ? null : list[0];
    }

    public static async Task<T?> FirstOrNull<T>(this Task<List<T>> task, Func<T, bool> lambda) where T : class
    {
        var list = await task;
        return list.Where(lambda).ToList().Count.Equals(0) ? null : list[0];
    }
}

public class PhoneDbLoadHelper : IPhoneDbServiceLoader
{
    private PhoneDbContext _Context;

    public PhoneDbLoadHelper(PhoneDbContext context)
    {
        this._Context = context;
    }

    /*public Task<Section?> GetSection(int sectionId)
    {
        return _Context.Sections.Where(item => item.Equals(sectionId)).ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());
    }*/


    /**
    * <summary>
    * Will load <c>Phone</c> <param>phoneId</param> and all reachable navigation properties from <c>Phone</c>  into <param>Db</param>.
    * However <c>PhoneReviews</c>,<c>PropertyTypes</c>, <c>SectionTypes</c>  navigation properties will not be loaded.
    * </summary>
    */
    public async Task<bool> LoadPhonePropsIntoContext(int phoneId)
    {
        var phone = await _Context.Phones
            // Images
            .Include(phone => phone.Images)

            //Boolean Properties
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.BooleanProperties)

            //Double Properties
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.DoubleProperties)

            //Long Properties
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.LongProperties)

            //Date Properties
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.DateProperties)

            //String Properties
            .Include(phoneItem => phoneItem.Sections)
            .ThenInclude(section => section.StringProperties).Where(phone => phone.Id.Equals(phoneId))
            .ToListAsync().ContinueWith(item => item.Result.FirstOrNull());

        return phone is not null;
    }

    /**
      * <summary>
      * Will load all <c>BooleanPropertyTypes</c>,<c>DoublePropertyTypes</c>,<c>LongPropertyTypes</c>,<c>StringPropertyTypes</c> and <c>SectionTypes</c> into <param>Db</param>.
      * </summary>
      */
    public async Task LoadAllTypesIntoContext()
    {
        await _Context.BooleanPropertyTypes.LoadAsync();
        await _Context.DoublePropertyTypes.LoadAsync();
        await _Context.LongPropertyTypes.LoadAsync();
        await _Context.StringPropertyTypes.LoadAsync();
        await _Context.DatePropertyTypes.LoadAsync();
        await _Context.SectionTypes.LoadAsync();
    }

    public Task LoadAllBooleanPropertyTypesIntoContext()
    {
        return _Context.BooleanPropertyTypes.ToListAsync();
    }

    public Task LoadAllStringPropertyTypesIntoContext()
    {
        return _Context.StringPropertyTypes.ToListAsync();
    }

    public Task LoadAllDoublePropertyTypesIntoContext()
    {
        return _Context.DoublePropertyTypes.ToListAsync();
    }

    public Task LoadAllDatePropertyTypesIntoContext()
    {
        return _Context.DatePropertyTypes.ToListAsync();
    }

    public Task LoadAllLongPropertyTypesIntoContext()
    {
        return _Context.LongPropertyTypes.ToListAsync();
    }

    public Task LoadAllSectionTypesIntoContext()
    {
        return _Context.SectionTypes.ToListAsync();
    }


    /*public Task<bool> IsBooleanPropertyTypeUsedInSection(int propertyTypeId, int sectionId)
    {
        return Task.FromResult(_Context.Sections
            .Any(section => section.Id.Equals(sectionId) &&
                            section.BooleanProperties.Any(property => property.Equals(propertyTypeId))));
    }*/

    /*public Task<bool> IsBooleanPropertyTypeInsertableInSection(int propertyTypeId, int sectionId)
    {
        return Task.FromResult(_Context.Sections
            .Any(section => section.Id.Equals(sectionId) &&
                            section
                                .SectionType
                                .BooleanPropertyTypes
                                .Any(propertyType => propertyType.Id.Equals(propertyTypeId))));
    }*/

    /*public async Task<bool> AddBooleanProperty(int propertyTypeId, int sectionId, bool value)
    {
        var propertyType = await _Context.BooleanPropertyTypes
            .Where(item => item.Id.Equals(propertyTypeId))
            .ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());

        if (propertyType is null)
        {
            return false;
        }


        var section = await _Context.Sections
            .Where(item => item.Id.Equals(sectionId)).ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());

        if (section is null)
        {
            return false;
        }


        _Context.BooleanProperties.Add(new BooleanProperty()
        {
            BooleanPropertyType = propertyType,
            Section = section,
            BoolData = value
        });

        try
        {
            await _Context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            if (e is DbUpdateConcurrencyException)
            {
                return false;
            }

            throw;
        }
    }*/

    /*public async Task<Phone> GetCompletePhoneByPropertyId(int sectionId){}*/

    /*public Task<Phone?> FindPhoneBySectionId(int sectionId)
    {
        return _Context.Phones
            .Where(phone => phone.Sections.Any(section => section.Id.Equals(sectionId)))
            .ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());
    }

    public Task<Phone?> FindPhoneByPropertyId(int propertyId)
    {
        return _Context.Phones.Where(item =>
                item.Sections.Any(section =>
                    section.BooleanProperties.Any(property => property.Id.Equals(propertyId)) ||
                    section.DoubleProperties.Any(property => property.Id.Equals(propertyId)) ||
                    section.StringProperties.Any(property => property.Id.Equals(propertyId)) ||
                    section.DateProperties.Any(property => property.Id.Equals(propertyId)) ||
                    section.LongProperties.Any(property => property.Id.Equals(propertyId))
                )
            ).ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());
    }

    public Task<BooleanPropertyType?> GetBooleanPropertyType(int propertyTypeId)
    {
        return _Context.BooleanPropertyTypes.Where(item => item.Id.Equals(propertyTypeId)).ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());
    }

    public Task<BooleanPropertyType?> S(int propertyTypeId)
    {
        return _Context.BooleanPropertyTypes.Where(item => item.Id.Equals(propertyTypeId)).ToListAsync()
            .ContinueWith(item => item.Result.FirstOrNull());
    }

    public async Task<string?> CheckIfBooleanPropAddValidRequest(int sectionId, int propertyTypeId)
    {
        var unknownSectionId = !await SectionExist(sectionId);

        if (unknownSectionId)
        {
            return "No section with id {sectionId} exist.";
        }

        var phone = await FindPhoneBySectionId(sectionId);
        if (phone is null)
        {
            return $"No phone is found belonging to sectionId {sectionId}.";
        }

        var unknownPropertyType = !await BooleanPropertyTypeExist(propertyTypeId);

        if (unknownPropertyType)
        {
            return $"No propertyType with id {propertyTypeId} exist.";
        }

        var propertyTypeIsNotInsertAbleInSection =
            !await IsBooleanPropertyTypeInsertableInSection(propertyTypeId, sectionId);

        if (propertyTypeIsNotInsertAbleInSection)
        {
            return $"PropertyType with id {propertyTypeId} is not insertable in section with id {sectionId}";
        }

        return null;
    }*/
}