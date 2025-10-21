using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.PhoneReview;

public class Index : PageModel
{
    private readonly PhoneDbContext _context;
    public Index(PhoneDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true),Required] public int PhoneId { get; set; }

    [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 20;

    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 0;

    public List<DataBase.PhoneReview> Reviews { get; set; } = new();
    
    public Phone? Phone { get; set; }
    public int TotalPages { get; set; }
    public int MaxPhonesPerPages = 50;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        PageSize = int.Min(int.Max(PageSize, 0), MaxPhonesPerPages);
        var phoneId = PhoneId;
        
        var query = from review in  _context.PhoneReviews.AsNoTracking() 
            where review.PhoneId.Equals(phoneId)
            orderby review.CreateDate
            select review;
        
        var items = from item  in query.Skip(PageIndex * PageSize).Take(PageSize) select item;

        Phone = await _context.Phones.FindAsync(phoneId);
        
        Reviews.AddRange(await items.ToListAsync());
       
        Console.WriteLine($"Reviews:{Reviews.Count}");
        
        // Calculate total pages for navigation if needed
        TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);
        return Page();
    }
}