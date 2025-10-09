using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PhoneReview;

public class Detail : PageModel
{
    private readonly PhoneDbContext _context;

    public Detail(PhoneDbContext context)
    {
        _context = context;
    }

    [Required,BindProperty(SupportsGet = true)] public int? PhoneReviewId { get; set; }

    public DataBase.PhoneReview? Review { get; set; } 

    public async Task<IActionResult> OnGet()
    {

        if (!ModelState.IsValid)
        {
            return Page();
        }
        if (PhoneReviewId is null)
        {
            throw new InvalidOperationException();
        }

        // Fetch the review from the database
        var review = await _context.PhoneReviews
            .Include(item => item.Phone)
            .FirstOrDefaultAsync(r => r.Id == PhoneReviewId);

        if (review is null)
        {
            ModelState.AddModelError(nameof(Detail.Review), "Review is null");
        }

        Review = review;
        return Page();
    }
}
