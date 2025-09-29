using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace PhoneDB.Pages.SectionType
{
    public class IndexModel : PageModel
    {
        private readonly DataBase.ApplicationDbContext _context;

        public IndexModel(DataBase.ApplicationDbContext context)
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
