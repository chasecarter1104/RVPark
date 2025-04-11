using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
namespace RVPark.Pages.Admin.Sites
{

    //------------------------------------------NOTES HERE
    // most data read only, insertable pictures.
    //
    public class DetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webhhostEnvironment;

        [BindProperty]
        public Site SiteObj { get; set; }
        [BindProperty]
        public IFormFile? ImageFile { get; set; }


        public IEnumerable<SelectListItem> SiteTypeList { get; set; }

        public DetailsModel(UnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webhhostEnvironment = hostEnvironment;
        }

        public void OnGet(int? id)
        {
            LoadSiteTypes();

            if (id != null) // edit version
            {
                SiteObj = _unitOfWork.Site.Get(u => u.Id == id) ?? new Site();
            }
            else
            {
                SiteObj = new Site();
            }
        }

        public IActionResult OnPost()
        {
            LoadSiteTypes();
            string webRootPath = _webhhostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            /*if (!ModelState.IsValid)
            {
                return Page();
            }

            if (SiteObj.Id == 0) // if new
            {
                //_unitOfWork.Site.Add(SiteObj);
            }
            else // existing
            {
                //_unitOfWork.Site.Update(SiteObj);
            }

            //_unitOfWork.Commit();*/
            return RedirectToPage("./Index");
        }

        private void LoadSiteTypes()
        {
            var siteTypes = _unitOfWork.SiteType.List();
            SiteTypeList = siteTypes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
        }
    }
}
