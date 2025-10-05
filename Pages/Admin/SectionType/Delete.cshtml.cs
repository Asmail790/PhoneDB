using System.ComponentModel.DataAnnotations;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PhoneDB.Pages.Admin.SectionType
{
    public class DeleteModel : PageModel
    {
        [Required, BindProperty(SupportsGet = true)]
        public int? SectionTypeId { get; set; }

        public DataBase.SectionType? SectionType { get; set; } = null;

        private readonly DataBase.PhoneDbContext _context;

        public DeleteModel(DataBase.PhoneDbContext context)
        {
            _context = context;
        }

        private async Task ValidateRequest()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            if (this.SectionTypeId is null)
            {
                throw new InvalidOperationException();
            }

            int sectionTypeId = SectionTypeId.Value;

            var sectionType = await _context.SectionTypes.Where(item => item.Id.Equals(sectionTypeId)).ToListAsync()
                .FirstOrNull();
            if (sectionType is null)
            {
                ModelState.AddModelError(nameof(SectionTypeId), $"SectionTypeId dont exist {sectionTypeId}.");
            }

            SectionType = sectionType;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            await ValidateRequest();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            await ValidateRequest();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (SectionType is null)
            {
                throw new InvalidOperationException();
            }

            _context.SectionTypes.Remove(SectionType);

            return await Utils.Utils.SaveAndRedirectIfSuccessful(
                this, _context, ModelState, RedirectToPage("./Index")
            );
        }
    }
}