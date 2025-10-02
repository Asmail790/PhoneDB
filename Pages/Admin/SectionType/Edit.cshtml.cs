using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace PhoneDB.Pages.SectionType
{
    public class EditModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;

        public EditModel(DataBase.PhoneDbContext context)
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

            var sectionname =  await _context.SectionTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (sectionname == null)
            {
                return NotFound();
            }
            SectionType = sectionname;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SectionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionNameExists(SectionType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SectionNameExists(int id)
        {
            return _context.SectionTypes.Any(e => e.Id == id);
        }
    }
}
