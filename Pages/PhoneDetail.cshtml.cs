using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PhoneDetail : PageModel
{
    public PhoneDbContext _dbContext;

    public PhoneDetail(PhoneDbContext dbContext)

    {
        _dbContext = dbContext;
    }

    [BindProperty(SupportsGet = true)] public int Id { get; set; }

    public void OnGet()
    {
    }
}