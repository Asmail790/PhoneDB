using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase;
using PhoneDB.Pages.Admin.PropertyType.Boolean;

namespace PhoneDB.Pages.SectionType
{
    public class EditModel : PageModel
    {
        private readonly DataBase.PhoneDbContext _context;

        
        [Required,BindProperty(SupportsGet = true) ] public int? SectionTypeId { get; set; }
        public DataBase.SectionType? SectionType { get; set; }

        public EditModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        private async Task ValidateRequest()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            if (SectionTypeId is null)
            {
                throw new InvalidOperationException();
            }
            var sectionTypeId = SectionTypeId.Value;
            var sectionType = await _context.SectionTypes
                .Include(sectionType => sectionType.BooleanPropertyTypes)
                .Include(sectionType => sectionType.DatePropertyTypes)
                .Include(sectionType => sectionType.StringPropertyTypes)
                .Include(sectionType => sectionType.DoublePropertyTypes)
                .Include(sectionType => sectionType.LongPropertyTypes)
                .Where(section => section.Id.Equals(sectionTypeId)).ToListAsync().FirstOrNull();
            if (sectionType is null)
            {
                ModelState.AddModelError(nameof(EditModel.SectionTypeId), $"{nameof(EditModel.SectionTypeId)} is not database.");
            }

            SectionType = sectionType;

        }

       

        public async Task<IActionResult> OnGetAsync()
        {
            await ValidateRequest();
            return Page();
        }
    }
}