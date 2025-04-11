using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
namespace RVPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTypeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public SiteTypeController(UnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.SiteType.List() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.SiteType.Get(c => c.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.SiteType.Delete(objFromDb);
            _unitOfWork.Commit();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpPost("lock/{id}")]  // This makes the endpoint /api/siteType/lock/{id}
        public IActionResult Lock(int id)
        {
            var objFromDb = _unitOfWork.SiteType.GetByID(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Site Type not found." });
            }

            objFromDb.IsLocked = !objFromDb.IsLocked; // Toggle lock state
            _unitOfWork.SiteType.Update(objFromDb);
            _unitOfWork.Commit();

            return Json(new { success = true, message = objFromDb.IsLocked ? "Site Type locked successfully." : "Site Type unlocked successfully." });
        }
    }
}
