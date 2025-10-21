using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.PhoneReviews;

public class Delete : PageModel
{


    private readonly PhoneDbContext _context;

    public Delete(PhoneDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true), Required] public int PhoneReviewId { get; set; }

    public DataBase.PhoneReview? PhoneReview { get; set; }

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        var phoneReview = await _context.PhoneReviews.Include(item => item.Phone).Where( item => item.Id.Equals(PhoneReviewId)).ToListAsync().FirstOrNull();
        
        if (phoneReview == null)
        {
            ModelState.AddModelError(string.Empty, "The Phone Review does not exist.");
        }

        PhoneReview = phoneReview;
    }
    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var phoneReview = PhoneReview;
        if (phoneReview is null)
        {
            throw new Exception();
        }

        _context.PhoneReviews.Remove(phoneReview);

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("/PhoneDetail", new { phoneId = phoneReview.PhoneId }));
    }
}