using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.PhoneReview;

public class Create : PageModel
{
    private PhoneDbContext _context;
    private int _fakeUserId;

    public Create(PhoneDbContext context)
    {
        _context = context;
        Random rnd = new Random();
        _fakeUserId = rnd.Next();
    }

    [BindProperty(SupportsGet = true), Required]
    public int PhoneId { get; set; }

    [BindProperty,Required,Range(1,5, ErrorMessage = "Score must be between 1 and 5.")] public int? Score { get; set; }
    [BindProperty, Required] public string? Description { get; set; } = null;
    [BindProperty, Required] public string? Title { get; set; } = null;

    Phone? Phone { get; set; }


    private async Task ValidateRequest()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        var phone = await _context.Phones.FindAsync(PhoneId);
        Phone = phone;

        if (phone is null)
        {
            ModelState.AddModelError(nameof(PhoneId), $"Phone with id {PhoneId} not found.");
            return;
        }


        var phoneReviewExist = await
            _context.PhoneReviews.AnyAsync(review => review.User.Equals(_fakeUserId) && review.PhoneId.Equals(PhoneId));
        if (phoneReviewExist)
        {
            ModelState.AddModelError(string.Empty, $"PhoneReview with id {PhoneId} is already created.");
            return;
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await ValidateRequest();
        if (!ModelState.IsValid)
        {
            return Page();
        }


        if (Score is null)
        {
            throw new Exception();
        }

        var score = Score.Value;

        var title = Title;
        if (title is null)
        {
            throw new Exception();
        }

        var description = Description;
        if (description is null)
        {
            throw new Exception();
        }

        var phone = Phone;
        if (phone is null)
        {
            throw new Exception();
        }

        DataBase.PhoneReview review = new()
        {
            Title = title,
            Description = description,
            Phone = phone,
            User = _fakeUserId
        };

        _context.Add(review);

        return await Utils.Utils.SaveAndRedirectIfSuccessful(this, _context, this.ModelState,
            RedirectToPage("/PhoneDetail", new { phoneId = phone.Id }));
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await ValidateRequest();
        return Page();
    }
}