using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
namespace RVPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SiteController(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.Site.List(null, null, "SiteType") });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Site.GetByID(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.Site.Delete(objFromDb);
            _unitOfWork.Commit();
            return Json(new { success = true, message = "Delete successful." });
        }


        [HttpPost]
        [Route("api/site/lockunlock/{id}")]
        public async Task<IActionResult> LockUnlock(int id)
        {
            var site = _unitOfWork.Site.Get(s => s.Id == id);
            if (site == null)
            {
                return NotFound(new { success = false, message = "Site not found." });
            }

            if (site.IsLocked == null || site.IsLocked == false) // Lock the site
            {
                site.IsLocked = true;
            }
            else // Unlock the site
            {
                site.IsLocked = false;
            }

            _unitOfWork.Site.Update(site);
            await _unitOfWork.CommitAsync();

            return Ok(new { success = true, message = $"Site {(site.IsLocked ? "locked" : "unlocked")} successfully." });
        }

    }
}
