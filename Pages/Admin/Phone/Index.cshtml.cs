namespace  PhoneDB.Admin.Pages.Phone;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBase;


    public class IndexModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;

        public IndexModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        public IList<DataBase.Phone> Phone { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Phone = await _context.Phones.ToListAsync();
        }
    }
