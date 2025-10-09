using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PhoneReview;

public class Delete : PageModel
{
    private readonly PhoneDbContext _context;

    public Delete(PhoneDbContext context)
    {
        _context = context;
    }

    [Required,BindProperty(SupportsGet = true)] public int? PhoneReviewId { get; set; }

    public DataBase.PhoneReview? PhoneReview { get; set; }
    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        if (PhoneReviewId is null)
        {
            throw new InvalidOperationException();
        }

        var phoneReviewId = PhoneReviewId.Value;

        PhoneReview = await _context.PhoneReviews
            .Include(item => item.Phone)
            .SingleOrDefaultAsync(item => item.Id.Equals(phoneReviewId));
        if (PhoneReview is null)
        {
            ModelState.AddModelError(nameof(Delete.PhoneReview),"PhoneReview not found");
        }
    }
    public async Task<IActionResult> OnGetAsync(int id)
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
            throw new InvalidOperationException();
        }

        _context.PhoneReviews.Remove(phoneReview);
        return await Utils.Utils.SaveAndRedirectIfSuccessful(this,_context,ModelState,RedirectToPage("./Index"));
    }
}