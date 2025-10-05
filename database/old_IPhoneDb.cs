namespace DataBase;


// old
public interface IPhoneDbSaveChange
{
    Task<bool> SaveChange();
}
public enum PropertyTypeEnum
{
    Boolean,
    String,
    Double,
    Long,
    Date,
}

public enum GetPhoneByEnum
{
    PhoneId,
    PropertyId,
    SectionId,
    ImageId,
    ReviewId,
}

public enum EntityTypeEnum
{
    BooleanPropertyType,
    StringPropertyType,
    DoublePropertyType,
    LongPropertyType,
    DatePropertyType,
    BooleanProperty,
    StringProperty,
    DoubleProperty,
    LongProperty,
    DateProperty,
    Phone,
    Image,
    Section,
    SectionType
}

public interface IPoneDbGetSingleItemQuery
{
    Task<Phone?> GetPhone(int id);
    Task<Section?> GetSection(int id);
    Task<SectionType?> GetSectionType(int id);
    Task<PhoneImage?> GetImage(int id);
    Task<PhoneReview?> GetReview(int id);

    Task<PropertyType?> GetPropertyType(PropertyTypeEnum type, int id);
    Task<Property?> GetStringProperty(PropertyTypeEnum type, int id);
}

public interface IPoneDbExistsQuery
{
    Task<bool> Exists(EntityTypeEnum type, int id);
}

// will not call SavaChange
public interface IPoneDbDeleteQuery
{
    Task<bool> Delete(EntityTypeEnum entityType, int id);
}

// will not call SavaChange
public interface IPoneDbInsertQuery
{
    Task Insert(EntityTypeEnum entityType, object obj);
}

public interface IPoneDbExistsWithRelationShipQuery
{
    Task<bool> PropertyExistsInSectionId(PropertyTypeEnum type, int propertyId, int sectionId);
    Task<bool> PropertyTypeExistsInSectionType(PropertyTypeEnum type, int propertyType, int sectionId);
    Task<bool> PropertyWithTypeIdExistsInSectionId(PropertyTypeEnum type, int propertyTypeId, int sectionId);
    Task<bool> PropertyWithTypeIdExistsInPhone(PropertyTypeEnum type, int propertyTypeId, int phoneId);
    Task<bool> SectionOfSectionTypeExistInPhone(int sectionType, int phoneId);
}

public interface IPoneDbGetWithRelationShipQuery
{
    Task<Phone?> GetPhoneBy(GetPhoneByEnum by, int id);
    Task<Section?> GetSectionByPropertyId(PropertyType type, int propertyId);
    Task<Property?> GetPropertyOfPropertyTypeInSection(PropertyType type, int propertyType, int sectionId);
    Task<Property?> GetPropertyOfPropertyTypeInPhone(PropertyType type, int propertyType, int phone);
    Task<Section?> GetSectionOfSectionTypeInPhone(int sectionTypeId, int phone);
    
}

public interface IPoneDbGetMultipleWithRelationShipQuery
{
    public Task<List<Phone>> GetPhones(List<PropertyTypeEnum>c,int pageSize,int section );
    public Task<int> TotalPages(List<PropertyTypeEnum>c);
    // Get By Releationship and multi, get total match
    // Get paged subset Phones fullfining [... proeprtyType of propertyValue with value Value] for each phone order by Id.
    // Get  total Phones fullfining [... proeprtyType of propertyValue with value Value] more than x pages where a page is y..
}

public interface IPoneDbGetMultipleQuery
{
    // Get Multi Items, Get total match 

    // Get paged subset properties with PropertyTypeID Ordered By PropertyTypeID.Name
    public Task<List<Property>> GetPropertiesWIthPropertyType(int propertyType,int pageSize,int page);
    public Task<List<Property>> GetPropertiesAllInSection(PropertyType type, int sectionId,int pageSize,int page);
    public Task<List<Property>> GetPhonesWithPropertyTypeId(PropertyType type, int propertyTypeId,int pageSize,int page);
}