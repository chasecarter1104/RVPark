using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
namespace RVPark.Pages.Admin.Fees
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhhostEnvironment;

        [BindProperty]
        public Fee FeeObj { get; set; }

        public IEnumerable<SelectListItem> FeeObjList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhhostEnvironment = hostEnvironment;
        }

        public void OnGet(int? id)
        {

            if (id != null) // edit version
            {
                FeeObj = _unitOfWork.Fee.Get(u => u.Id == id) ?? new Fee();
            }
            else
            {
                FeeObj = new Fee();
            }
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (FeeObj.Id == 0) // if new
            {
                _unitOfWork.Fee.Add(FeeObj);
            }
            else // existing
            {
                _unitOfWork.Fee.Update(FeeObj);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }

    }
}
