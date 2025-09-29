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
    public class DeleteModel : PageModel
    {
        private readonly DataBase.ApplicationDbContext _context;

        public DeleteModel(DataBase.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DataBase.SectionType SectionType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
          
            if (id == null)
            {
                return NotFound();
            }

            var sectionname = await _context.SectionTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (sectionname is not null)
            {
                SectionType = sectionname;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectionname = await _context.SectionTypes.FindAsync(id);
            if (sectionname != null)
            {
                SectionType = sectionname;
                _context.SectionTypes.Remove(SectionType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
