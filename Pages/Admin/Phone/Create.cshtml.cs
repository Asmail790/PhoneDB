using System.ComponentModel.DataAnnotations;

namespace PhoneDB.Pages.Admin.Phone;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBase;

public class CreateModel : PageModel
{
    private readonly DataBase.PhoneDbContext _context;
    
    [Required,BindProperty(SupportsGet = true)] public string? Name { get; set; } 

    public CreateModel(DataBase.PhoneDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }
    
    // For more information, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Name is null)
        {
            throw new Exception();
        }

        var phone = new Phone()
        {
            Name = Name
        };
        

        _context.Phones.Add(phone);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}