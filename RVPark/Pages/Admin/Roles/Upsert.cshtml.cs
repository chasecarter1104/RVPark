using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
namespace RVPark.Pages.Admin.Roles
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;

        [BindProperty]
        public Role RoleObj { get; set; }

        public IEnumerable<SelectListItem> RoleObjList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, RoleManager<Identity> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public void OnGet(string? id)
        {
            if (!string.IsNullOrEmpty(id)) // edit version
            {
                RoleObj = _unitOfWork.Role.Get(u => u.Id == id) ?? new Role();
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

            if (string.IsNullOrEmpty(RoleObj.Id)) // if new
            {
                RoleObj.Id = Guid.NewGuid().ToString(); // or let Identity generate one
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