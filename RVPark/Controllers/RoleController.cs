using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace RVPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
        {
            private readonly UnitOfWork _unitOfWork;
            private readonly IWebHostEnvironment _webHostEnvironment;

            public RoleController(UnitOfWork UnitOfWork) => _unitOfWork = UnitOfWork;

            [HttpGet]
            public IActionResult Get()
            {
                return Json(new { data = _unitOfWork.Role.List() });
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var objFromDb = _unitOfWork.Role.GetByID(id);
                if (objFromDb == null)
                {
                    return Json(new { success = false, message = "Error while deleting." });
                }
                _unitOfWork.Role.Delete(objFromDb);
                _unitOfWork.Commit();
                return Json(new { success = true, message = "Delete successful." });
            }

            
        [HttpPost("lock/{id}")]  // This makes the endpoint /api/role/lock/{id}
        public IActionResult Lock(int id)
        {
            var objFromDb = _unitOfWork.Role.GetByID(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Role not found." });
            }

            objFromDb.IsLocked = !objFromDb.IsLocked; // Toggle lock state
            _unitOfWork.Role.Update(objFromDb);
            _unitOfWork.Commit();

            return Json(new { success = true, message = objFromDb.IsLocked ? "Role locked successfully." : "Role unlocked successfully." });
        }


    }
}

