using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.PhoneCompare;

public class PropertyTypeComparer : IEqualityComparer<DataBase.PropertyType>
{
    public bool Equals(DataBase.PropertyType? x, DataBase.PropertyType? y)
    {
        return x?.Id == y?.Id;
    }

    public int GetHashCode(DataBase.PropertyType obj)
    {
        return obj.Id;
    }
}

public class SectionTypeComparer : IEqualityComparer<DataBase.SectionType>
{
    public bool Equals(DataBase.SectionType? x, DataBase.SectionType? y)
    {
        return x?.Id == y?.Id;
    }

    public int GetHashCode(DataBase.SectionType obj)
    {
        return obj.Id;
    }
}


public class Index : PageModel
{
    [Required,BindProperty(SupportsGet = true)] public int? PhoneId0 { get; set; }

    [Required,BindProperty(SupportsGet = true)] public int? PhoneId1 { get; set; }

    private PhoneDbContext _context;

    public Phone? Phone0 { get; set; }
    public Phone? Phone1 { get; set; }

    public Index(PhoneDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (PhoneId0 is null)
        {
            throw new Exception();
        }

        int phoneId0 = PhoneId0.Value;
        if (PhoneId1 is null)
        {
            throw new Exception();
        }

        int phoneId1 = PhoneId1.Value;

        var phoneLoader = new DataBase.PhoneDbLoadHelper(_context);
        if (!await phoneLoader.LoadPhonePropsIntoContext(phoneId0))
        {
            ModelState.AddModelError(nameof(PhoneId0), "Not in database");
            return Page();
        }

        if (!await phoneLoader.LoadPhonePropsIntoContext(phoneId1))
        {
            ModelState.AddModelError(nameof(PhoneId1), "Not in database");
            return Page();
        }

        await phoneLoader.LoadAllTypesIntoContext();

        Phone0 = await _context.Phones.FindAsync(phoneId0);
        Phone1 = await _context.Phones.FindAsync(phoneId1);

        return Page();
    }
}