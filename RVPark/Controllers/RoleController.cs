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
    }
}

