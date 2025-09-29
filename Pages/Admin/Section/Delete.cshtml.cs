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
    public class DeleteModel(DataBase.ApplicationDbContext context) : PageModel
    {
        [BindProperty] public DataBase.Section Section { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await context
                .Sections
                .Include(item => item.SectionType)
                .Include(item => item.Phone)
                .Where(item => item.Id == id).FirstOrDefaultAsync();

            if (section is not null)
            {
                Section = section;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var section = await context
                .Sections.Include(item => item.SectionType)
                .Include(item => item.Phone)
                .Where(item => item.Id == id).FirstOrDefaultAsync();

            if (section != null)
            {
                Section = section;
                context.Sections.Remove(Section);
                await context.SaveChangesAsync();
            }

            if (section is null)
            {
                return NotFound();
            }

            return RedirectToPage("/Phone/Edit", new { id = section.Phone.Id });
        }
    }
}