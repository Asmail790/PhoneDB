using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages.Admin.Phone.Section.Property.Shared;




public abstract class PropertyCreator<T0,T1>:PageModel  where T1:PropertyType
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public int? SectionId { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public int? PropertyTypeId { get; set; }

    public T1? PropertyType { get; set; } = null;
    public DataBase.Section? Section { get; set; } = null;

    [BindProperty()]  public required T0  Value { get; set; }
}