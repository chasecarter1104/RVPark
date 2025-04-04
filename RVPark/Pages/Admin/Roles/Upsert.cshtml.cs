using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
namespace RVPark.Pages.Admin.Roles
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhhostEnvironment;

        [BindProperty]
        public Role RoleObj { get; set; }

        public IEnumerable<SelectListItem> RoleObjList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhhostEnvironment = hostEnvironment;
        }

        public void OnGet(int? id)
        {

            if (id != null) // edit version
            {
                RoleObj = _unitOfWork.Role.Get(u=>u.Id == id) ?? new Role();
            }
            else
            {
                RoleObj = new Role();
            }
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (RoleObj.Id == 0) // if new
            {
                _unitOfWork.Role.Add(RoleObj);
            }
            else // existing
            {
                _unitOfWork.Role.Update(RoleObj);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }

    }
}
