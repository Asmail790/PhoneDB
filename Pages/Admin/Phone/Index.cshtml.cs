using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace  PhoneDB.Pages.Admin.Phone;

public class IndexModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;
        [BindProperty(SupportsGet = true)] public int PageIndex { get; set; } = 0;
        [BindProperty(SupportsGet = true)] public string? PhoneName { get; set; }
        [BindProperty(SupportsGet = true)] public int? PhoneId { get; set; }
        [BindProperty(SupportsGet = true)] public int PageSize { get; set; } = 20;
        
        public int MaxPhonesPerPages = 20;
        public int TotalPages { get; set; }

        public IList<DataBase.Phone> Phones { get; set; } = new List<DataBase.Phone>();
        public IndexModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            PageSize = int.Min(int.Max(PageSize, 0), MaxPhonesPerPages);
            
            IQueryable<DataBase.Phone> query = _context.Phones.AsNoTracking()
                .OrderByDescending(item => item.Name);

            if (PhoneId is not null)
            {
                query = query
                    .Where(item => PhoneId == null || item.Id == PhoneId);
            }
        
            if (!string.IsNullOrWhiteSpace(PhoneName))
            {
                query = query.Where(item => PhoneName == null || item.Name.ToLower().StartsWith(PhoneName) );
            }
        
            Phones = await query.Skip(PageIndex * PageSize).Take(PageSize).ToListAsync();
            
            TotalPages = (int)Math.Ceiling((double)query.Count() / PageSize);
            
        }
    }
