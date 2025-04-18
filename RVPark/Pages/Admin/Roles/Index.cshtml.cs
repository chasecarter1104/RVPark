using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RVPark.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;

        public IndexModel(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public IEnumerable<Role> RolesObj { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public async Task OnGetAsync(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
            RolesObj = _roleManager.Roles;

        }
    }

}