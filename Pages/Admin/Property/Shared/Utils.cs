using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PhoneDB.Pages.Admin.Phone.Section.Property.Shared;

namespace PhoneDB.Pages.Admin.Property.Shared;

public static class PropertyUtils
{

    private static string PhoneEditPath = "/Admin/Phone/Edit";

    public static RedirectToPageResult RedirectToPhoneEdit(PageModel pageModel,int phoneId)
    {
        return pageModel.RedirectToPage(PhoneEditPath, new { phoneId });
    }

    public static async Task ValidateAddPropertyRequest<T0, T1>(
        PropertyCreator<T0, T1> pageModel,
        PhoneDbContext context,
        Func<int, Task<T1?>> getPropertyTypeById,
        Func<(int PropertyTypeId, int sectionId), Task<bool>> isPropertyTypeNotIncludedInSectionType,
        Func<(int PropertyTypeId, int sectionId), Task<bool>> propertyWithSameInNameAndPropertyExist
    ) where T1 : PropertyType
    {
        if (!pageModel.ModelState.IsValid)
        {
            return;
        }

        if (pageModel.SectionId is null || pageModel.PropertyTypeId is null)
        {
            throw new InvalidOperationException();
        }

        var sectionId = pageModel.SectionId.Value;
        var propertyTypeId = pageModel.PropertyTypeId.Value;


        var section = await context.Sections
            .Include(section => section.SectionType)
            .Include(section => section.Phone)
            .Where(item => item.Id.Equals(sectionId)).ToListAsync().FirstOrNull();

        if (section is null)
        {
            pageModel.ModelState.AddModelError(nameof(PropertyCreator<T0, PropertyType>.SectionId),
                "sectionId not found");
            return;
        }

        pageModel.Section = section;

        var propertyType = await getPropertyTypeById(propertyTypeId);
        //await context.BooleanPropertyTypes.Where(item => item.Id.Equals(propertyTypeId))
        //.ToListAsync()
        // .FirstOrNull();

        if (propertyType is null)
        {
            pageModel.ModelState.AddModelError(nameof(PropertyCreator<T0, PropertyType>.SectionId),
                "propertyId not found");
            return;
        }

        pageModel.PropertyType = propertyType;

        bool propertyTypeIsNotIncludedInSectionType =
            await isPropertyTypeNotIncludedInSectionType((propertyTypeId, sectionId));

        /*!await context.Sections.AnyAsync(item =>
        item.Id.Equals(sectionId) &&
        item.SectionType.BooleanPropertyTypes.Any(type => type.Id.Equals(propertyTypeId)));
        */

        if (propertyTypeIsNotIncludedInSectionType)
        {
            pageModel.ModelState.AddModelError(nameof(PropertyCreator<T0, PropertyType>.SectionId),
                "property type Id can not be inserted ");
            return;
        }

        bool propertyTypeUsedInSectionExist =
            await propertyWithSameInNameAndPropertyExist((propertyTypeId, sectionId));
        /*await context.BooleanProperties.AnyAsync(item =>
        item.SectionId.Equals(sectionId) && item.BooleanPropertyTypeId.Equals(propertyTypeId)
    );*/

        if (propertyTypeUsedInSectionExist)
        {
            pageModel.ModelState.AddModelError(nameof(PropertyCreator<T0, PropertyType>.PropertyTypeId),
                "A property with same propertyType exist already in section database");
        }
    }

    public static async Task ValidateEditRequest<T0, T1>(
        PropertyEditor<T0, T1> pageModel,
        Func<int, Task<T0?>> getPropertyById
    )
        where T0 : DataBase.Property
    {
        if (!pageModel.ModelState.IsValid)
        {
            return;
        }

        if (pageModel.PropertyId is null)
        {
            throw new InvalidOperationException();
        }

        var propertyId = pageModel.PropertyId.Value;

        var property = await getPropertyById(propertyId);
        /*await _context.BooleanProperties
        .Include(item => item.BooleanPropertyType)
        .Include(item => item.Section)
        .ThenInclude(item => item.SectionType)
        .Include(item => item.Section)
        .ThenInclude(section => section.Phone)
        .Where(item => item.Id.Equals(propertyId)).ToListAsync().FirstOrNull();*/
        if (property is null)
        {
            pageModel.ModelState.AddModelError(nameof(PropertyEditor<T0, T1>.PropertyId),
                "property is not in database of boolean properties.");
        }

        pageModel.Property = property;
    }

    public static Func<Task> CreatePropertyEditRequestValidator<T0, T1>(
        PropertyEditor<T0, T1> pageModel,
        Func<int, Task<T0?>> getPropertyById)
        where T0 : DataBase.Property
    {
        return () => PropertyUtils.ValidateEditRequest(pageModel, getPropertyById);
    }

    public static Func<Task> CreatePropertyAddRequestValidator<T0, T1>(
        PropertyCreator<T0, T1> pageModel,
        PhoneDbContext context,
        Func<int, Task<T1?>> getPropertyTypeById,
        Func<(int PropertyTypeId, int sectionId), Task<bool>> isPropertyTypeNotIncludedInSectionType,
        Func<(int PropertyTypeId, int sectionId), Task<bool>> isPropertyWithSameInNameAndPropertyExist
    ) where T1 : PropertyType => () => PropertyUtils.ValidateAddPropertyRequest(pageModel, context, getPropertyTypeById,
        isPropertyTypeNotIncludedInSectionType, isPropertyWithSameInNameAndPropertyExist);

    public static Func<Task> CreatePropertyDeleteRequestValidator<T>(
        PropertyRemover<T> pageModel,
        Func<int, Task<T?>> getPropertyById
    ) where T : DataBase.Property
    {
        return () => ValidatePropertyDeleteRequest(pageModel, getPropertyById);
    }

    public static async Task ValidatePropertyDeleteRequest<T>(
        PropertyRemover<T> pageModel,
        Func<int, Task<T?>> getPropertyById
    ) where T : DataBase.Property
    {
        if (!pageModel.ModelState.IsValid)
        {
            return;
        }


        if (pageModel.PropertyId is null)
        {
            throw new InvalidOperationException();
        }

        var propertyId = pageModel.PropertyId.Value;
        var property = await getPropertyById(propertyId);

        /*context.BooleanProperties
        .Include(item => item.BooleanPropertyType)
        .Include(item => item.Section)
        .ThenInclude(item => item.SectionType)
        .Include(item => item.Section)
        .ThenInclude(section => section.Phone)
        .Where(item => item.Id.Equals(propertyId)).ToListAsync().FirstOrNull();*/

        if (property is null)
        {
            pageModel.ModelState.AddModelError(string.Empty,
                $" {nameof(PropertyRemover<T>.Property)} Property with id {propertyId} not found.");
        }

        pageModel.Property = property;
    }
}