using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PhoneDetail : PageModel
{
    public ApplicationDbContext _dbContext;

    public PhoneDetail(ApplicationDbContext dbContext)

    {
        _dbContext = dbContext;
    }

    [BindProperty(SupportsGet = true)] public int Id { get; set; }

    public void OnGet()
    {
    }
}