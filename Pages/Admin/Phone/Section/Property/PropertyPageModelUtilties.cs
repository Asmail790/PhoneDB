using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Section.Property;

public static class PropertyPageModelUtilties
{
    public static async Task<IActionResult> SaveAndRedirectIfSucceses(
        PageModel model,
        PhoneDbContext context,
        ModelStateDictionary modelState,
        int phoneId,
        string errorMessage = "Problem with database.Try Again.")
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            modelState.AddModelError(string.Empty, errorMessage);
        }

        if (!modelState.IsValid)
        {
            return model.Page();
        }
        return model.RedirectToPage("/Admin/Phone/Edit", new { phoneId = phoneId });
    }


  
}