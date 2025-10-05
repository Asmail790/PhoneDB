using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.Admin.Property.Shared;

public abstract class PropertyEditor<T0, T1> : PageModel
    where T0 : DataBase.Property
{
    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyId { get; set; }

    [BindProperty()] public required T1 Value { get; set; }

    public T0? Property { get; set; } = null;
}