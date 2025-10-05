using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.SectionType
{
    public class CreateModel : PageModel
    {
        [Required, BindProperty(), MaxLength(200)] public string? Name { get; set; } = null;

        [BindProperty(),MaxLength(1000)] public string? Description { get; set; } = null;
        public DataBase.SectionType? SectionType { get; set; } = null;

        private readonly DataBase.PhoneDbContext _context;

        public CreateModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        private async Task<IActionResult> ValidateRequest()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Name is null)
            {
                throw new InvalidOperationException();
            }

            var name = Name;
            var sectionTypeWithSameNameExist =
                await _context.SectionTypes.AnyAsync(section => section.Name.Equals(name));

            if (sectionTypeWithSameNameExist)
            {
                ModelState.AddModelError(nameof(Name), $"A sectionType with name {name} already exists");
            }

            return Page();
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }
        
        


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await ValidateRequest();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (Name is null)
            {
                throw new InvalidOperationException();
            }

            string name = Name;
            string? description = Description;

            DataBase.SectionType sectionType = new DataBase.SectionType
            {
                Name = name,
                Description = description 
            };
            _context.SectionTypes.Add(sectionType);

            return await Utils.Utils.SaveAndRedirectIfSuccessful(
                this,
                _context,
                ModelState,
                RedirectToPage("./Index"));
        }
    }
}