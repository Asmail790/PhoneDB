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
        private readonly DataBase.PhoneDbContext _context;

        [Required]
        [BindProperty(SupportsGet = true)]
        public int PhoneId { get; set; }

        [Required]
        [BindProperty(SupportsGet = true)]
        public int SectionTypeId { get; set; }

        public string PhoneModel { get; set; } = null;
        public string SectionTypeName { get; set; } = null;

        public string? SectionDescription { get; set; } = null;
        public CreateModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var phoneMatches = await _context
                .Phones
                .Where(phone => phone.Id == PhoneId)
                .Select(p => new { p.Id, PhoneModel = p.Name }).ToListAsync();

            if (phoneMatches.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Phone not found");
            }
            else
            {
                PhoneModel = phoneMatches[0].PhoneModel;
            }

            var sectionTypeMatches = await _context.SectionTypes.Where(item => item.Id == SectionTypeId).ToListAsync();

            if (sectionTypeMatches.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Unknown SectionType not found");
            }

            var sectionType = sectionTypeMatches[0];
            SectionTypeName = sectionType.Name;
            SectionDescription = sectionType.Description;

            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var phoneMatches = await _context.Phones.Where(item => item.Id == PhoneId).ToListAsync();
            if (phoneMatches.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Phone not found");
            }

            var phone = phoneMatches[0];
            PhoneModel = phone.Name;

            var sectionTypeMatches = await _context.SectionTypes.Where(item => item.Id == SectionTypeId).ToListAsync();
            var sectionType = sectionTypeMatches[0];
            SectionTypeName = sectionType.Name;
            SectionDescription = sectionType.Description;


            if (sectionTypeMatches.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "SectionType not found");
            }

            var sectionMatches = await _context.Sections
                .Where(section => section.PhoneId == PhoneId && section.SectionTypeId == SectionTypeId)
                .ToListAsync();

            if (0 < sectionMatches.Count)
            {
                ModelState.AddModelError(string.Empty,
                    $"SectionType ${sectionType.Name} exist already in phoneModel ${phone.Name}");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Sections.Add(new DataBase.Section()
            {
                Phone = phone,
                SectionType = sectionType
            });

            await _context.SaveChangesAsync();


            return RedirectToPage("/Admin/Phone/Edit", new { phoneId = this.PhoneId });
        }
    }
}