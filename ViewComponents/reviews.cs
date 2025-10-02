using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ReviewsViewComponent : ViewComponent
{
    public PhoneDbContext _dbContext;

    public ReviewsViewComponent(PhoneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(int phoneId)
    {
        var reviews = await _dbContext.PhoneReviews.Where(r => r.PhoneId == phoneId).ToListAsync();


        var cards = reviews.Select(item =>
            (IReviewCard)new ReviewCard
            {
                PhoneReview = item,
                isUser = false
            }
        ).ToList();

        return View(cards);
    }
}