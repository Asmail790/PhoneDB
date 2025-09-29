using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBase;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Section
{
    public class CreateModel : PageModel
    {
        private readonly DataBase.ApplicationDbContext _context;
        [Required] [BindProperty()] public string? SectionName { get; set; } = null;

        [Required]
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; } = null;

        [BindProperty()] public string? PhoneModel { get; set; } = null;
        public SelectList? SelectList { get; set; } = null;
        
        


        public CreateModel(DataBase.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            if (Id == null)
            {
                return NotFound();
            }

            PhoneModel = await _context.Phones.Where(phone => phone.Id == Id).Select(item => item.PhoneModel)
                .FirstAsync();


            var unUsedSectionName = await _context.SectionTypes
                .Where(sectionName =>
                    !(_context.Sections
                        .Include(section => section.SectionType)
                        .Where(section => section.PhoneId.Equals(Id))
                        .Select(section => section.SectionType.Id).Any(keyName => keyName.Equals(sectionName.Id) ))
                ).ToListAsync();

            this.SelectList = new SelectList(unUsedSectionName, "Name", "Name");
            this.SectionName = _context.SectionTypes.First().Name;
            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return PopulateSelectListAndReturnPageResult();
            }

            DataBase.SectionType sectionKey =
                await _context.SectionTypes.Where(item => item.Id == this.Id).FirstAsync();
            var phone = await _context.Phones
                .Include(phone => phone.Sections)
                .ThenInclude(section => section.SectionType)
                .FirstAsync(phone => phone.Id == this.Id);

            var newSection = new DataBase.Section()
            {
                SectionType = sectionKey,
                Phone = phone
            };
            var sectionExistAllReady = phone.Sections.Exists((section) => section.SectionType.Equals(sectionKey));

            if (sectionExistAllReady)
            {
                this.SelectList = new SelectList(_context.SectionTypes, "Name", "Name");
                return PopulateSelectListAndReturnPageResult();
            }

            phone.Sections.Add(newSection);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Phone/Edit", new { id = this.Id });
        }

        private IActionResult PopulateSelectListAndReturnPageResult()
        {
            this.SelectList = new SelectList(_context.SectionTypes, "Name", "Name");
            return Page();
        }
    }
}