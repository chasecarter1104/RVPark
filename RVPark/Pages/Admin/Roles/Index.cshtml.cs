using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RVPark.Pages.Admin.Roles
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Role> Roles { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public void OnGet(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
            Roles = _unitOfWork.Role.List();
        }

       
    }
}
