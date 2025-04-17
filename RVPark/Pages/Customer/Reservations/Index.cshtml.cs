using System.Security.Claims;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RVPark.Pages.Customer.Reservations
{
    public class IndexModel(UnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        : PageModel
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public User User { get; set; }

        public async Task OnGetAsync(bool success = false, string message = null)
        {
            User = _unitOfWork.User.Get(u => u.Email == HttpContext.User.Identity.Name);
            if (User == null) User = new User();
        }
    }
}
