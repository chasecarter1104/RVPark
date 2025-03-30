using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;

namespace RVPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FeeController(UnitOfWork UnitOfWork) => _unitOfWork = UnitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            return Json( new { data = _unitOfWork.Fee.List() } );
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Reservation.GetByID(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.Reservation.Delete(objFromDb);
            _unitOfWork.Commit();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}
