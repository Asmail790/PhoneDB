namespace PhoneDB.Admin.Pages.Phone;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBase;


    public class DetailsModel : PageModel
    {
        private readonly DataBase.ApplicationDbContext _context;

        public DetailsModel(DataBase.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }

