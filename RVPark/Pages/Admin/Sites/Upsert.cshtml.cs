using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
namespace RVPark.Pages.Admin.Sites
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhhostEnvironment;

        [BindProperty]
        public Site Site { get; set; }


        public IEnumerable<SelectListItem> SiteTypeList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhhostEnvironment = hostEnvironment;
        }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                Site = _unitOfWork.Site.Get(u => u.Id == id, true);

                var SiteType = _unitOfWork.SiteType.List();

                SiteTypeList = SiteType.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            }
            if (Site == null) //new upsert
            {
                Site = new();
            }
        }

        public IActionResult OnPost(int? id)
        {
            if (Site.Id == 0)
            {
                // Add new reservation to the database
                _unitOfWork.Site.Add(Site);
            }
            else
            {
                // Update existing reservation
                var objFromDb = _unitOfWork.Site.Get(r => r.Id == Site.Id, true);
                _unitOfWork.Site.Update(Site);
            }

            // Save changes to the database
            _unitOfWork.Commit();

            // Redirect
            return RedirectToPage("./Index");
        }
    }
}
