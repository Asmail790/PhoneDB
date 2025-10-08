using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.SectionType
{
    public class IndexModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;

        public IndexModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        public IList<DataBase.SectionType> SectionName { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SectionName = await _context.SectionTypes.ToListAsync();
        }
    }
}
