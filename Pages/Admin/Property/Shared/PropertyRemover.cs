using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.Admin.Phone.Section.Property.Shared;

public abstract class PropertyRemover<T> : PageModel where T : DataBase.Property
{
    [Required, BindProperty(SupportsGet = true)]
    public int? PropertyId { get; set; }

    public T? Property { get; set; }
}