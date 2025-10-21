using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PhoneDB.Pages.PhoneReview;

public class Edit : PageModel
{
    private readonly PhoneDbContext _context;

    public Edit(PhoneDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true), Required] public int PhoneReviewId { get; set; }
    [BindProperty, Required] public string? Description { get; set; }

    [BindProperty, Required] public string? Title { get; set; }

    [BindProperty, Required, Range(1, 5)] public int? Score { get; set; }

    public DataBase.PhoneReview? PhoneReview { get; set; }

    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        var phoneReview = await _context.PhoneReviews
            .Include(item => item.Phone)
            .Where(item => item.Id.Equals(PhoneReviewId))
            .FirstOrDefaultAsync();

        if (phoneReview == null)
        {
            ModelState.AddModelError(string.Empty, "The Phone Review does not exist.");
        }

        PhoneReview = phoneReview;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        Description = PhoneReview?.Description;
        Score = PhoneReview?.Score;
        Title = PhoneReview?.Title;
        
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
            throw new Exception("Phone review not found.");
        }

        var description = Description;
        var title = Title;

        if (Score is null)
        {
            throw new Exception();
        }
        var score = Score.Value;

        if (description is null)
        {
            throw new Exception();
        }

        if (title is null)
        {
            throw new Exception();
        }

        phoneReview.Title = title;
        phoneReview.Description = description;
        phoneReview.Score = score;

        return await
        Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, ModelState, RedirectToPage("/PhoneDetail", new {phoneId = phoneReview.PhoneId}));


    }
}