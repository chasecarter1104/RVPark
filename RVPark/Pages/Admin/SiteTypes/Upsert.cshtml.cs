using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Data;
namespace RVPark.Pages.Admin.SiteTypes
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public SiteType SiteTypeObj { get; set; }
        public UpsertModel(UnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult OnGet(int? id)
        {
            SiteTypeObj = new SiteType();

            if (id != 0)
            {
                SiteTypeObj = _unitOfWork.SiteType.Get(u => u.Id == id);
                if (SiteTypeObj == null)
                {
                    return NotFound();
                }

            }
            return Page(); //assume insert new mode
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //If New
            if (SiteTypeObj.Id == 0)
            {
                _unitOfWork.SiteType.Add(SiteTypeObj);
            }
            //existing
            else
            {
                _unitOfWork.SiteType.Update(SiteTypeObj);
            }
            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
