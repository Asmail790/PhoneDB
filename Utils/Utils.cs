using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace PhoneDB.Utils;

public static class Utils
{
    public static async Task<IActionResult> SaveAndRedirectIfSuccessful(
        PageModel model,
        PhoneDbContext context,
        ModelStateDictionary modelState,
        RedirectToPageResult redirect,
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

        return redirect;
    }


    public static string IfNullReturnUnknownString(object? obj)
    {
        if (obj is  null)
        {
            return "Unknown";
        }
        return obj.ToString();
    }


    public static string GetClientPathForImage(string filename)
    {
        return $"/images/{filename}";
    }
}