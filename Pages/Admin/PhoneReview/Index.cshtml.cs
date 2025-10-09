using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.PhoneReview;

public class Index : PageModel
{
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 0;
    [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 20;
    [BindProperty(SupportsGet = true)] public int? PhoneId { get; set; }
    
    [BindProperty(SupportsGet = true)] public string? PhoneName { get; set; }
    
    [BindProperty(SupportsGet = true)] public string? UserName { get; set; }

    [BindProperty(SupportsGet = true)] public int? UserId { get; set; }
    public int MaxPhonesPerPages = 50;
    public int TotalPages { get; set; }

    public List<DataBase.PhoneReview> Reviews { get; set; } = new();


    private PhoneDbContext _context;

    public Index(PhoneDbContext context)
    {
        _context = context;
    }

    public async Task OnGetAsync()
    {
        PageSize = int.Min(int.Max(PageSize, 0), MaxPhonesPerPages);

        IQueryable<DataBase.PhoneReview> query = _context.PhoneReviews.AsNoTracking()
            .Include(item => item.Phone)
            .OrderBy(item => item.CreateDate);

        if (PhoneId is not null)
        {
            query = query
                .Where(item => PhoneId == null || item.PhoneId == PhoneId);
        }

        if (UserId is not null)
        {
            query = query.Where(item => UserId == null || item.User == UserId);
        }

        if (!string.IsNullOrWhiteSpace(UserName))
        {
            // TODO 
            query = query.Where(item => true);
        }
        
        if (!string.IsNullOrWhiteSpace(PhoneName))
        {
            query = query.Where(item => PhoneName == null || item.Phone.Name.ToLower().StartsWith(PhoneName) );
        }
        
        Reviews = await query.Skip(PageIndex * PageSize).Take(PageSize).ToListAsync();

        // Calculate total pages for navigation if needed
        TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);
    }
}