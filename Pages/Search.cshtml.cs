using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages;


public class SearchModel : PageModel
{
    private readonly DataBase.PhoneDbContext _context;

    public SearchModel(DataBase.PhoneDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)] public int? PhoneId { get; set; }
    [BindProperty(SupportsGet = true)] public string? PhoneName { get; set; }
    [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 0;
    [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 20;

    public List<DataBase.Phone> Phones { get; set; } = new();
    public int TotalPages { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var pageSize = int.Min(int.Max((PageSize), 1), 20);
        var pageIndex = int.Max(PageIndex, 0);

        IQueryable<DataBase.Phone> query = _context.Phones.AsNoTracking().Include(phone => phone.Thumbnail).OrderByDescending(phone => phone.Name);
        
        if (PhoneId.HasValue)
        {
            query = query.Where(phone => phone.Id == PhoneId.Value);
        }

        if (!string.IsNullOrWhiteSpace(PhoneName))
        {
            query = query.Where(phone => phone.Name.ToLower().StartsWith(PhoneName.ToLower()));
        }

        var totalCount = await query.CountAsync();
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        Phones = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return Page();
    }
}