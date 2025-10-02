namespace PhoneDB.Pages.Admin.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBase;


    public class DeleteModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;

        public DeleteModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DataBase.Phone Phone { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phone = await _context.Phones.FirstOrDefaultAsync(m => m.Id == id);

            if (phone is not null)
            {
                Phone = phone;

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

            var phone = await _context.Phones.FindAsync(id);
            if (phone != null)
            {
                Phone = phone;
                _context.Phones.Remove(Phone);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }

