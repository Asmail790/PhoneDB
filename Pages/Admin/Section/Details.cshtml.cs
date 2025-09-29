using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace PhoneDB.Pages.Section
{
    public class DetailsModel : PageModel
    {
        private readonly DataBase.ApplicationDbContext _context;

        public DetailsModel(DataBase.ApplicationDbContext context)
        {
            _context = context;
        }

        public DataBase.Section Section { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.Sections.FirstOrDefaultAsync(m => m.Id == id);

            if (section is not null)
            {
                Section = section;

                return Page();
            }

            return NotFound();
        }
    }
}
