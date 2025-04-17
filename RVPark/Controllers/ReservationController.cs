using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
namespace RVPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReservationController(UnitOfWork UnitOfWork) => _unitOfWork = UnitOfWork;

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _unitOfWork.Reservation.List(null, null, "Site,User");
                return Json(new { data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
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

        [HttpGet("AvailableSites")]
        public IActionResult GetAvailableSites([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return BadRequest(new { success = false, message = "Start date and end date are required." });
            }

            if (startDate > endDate)
            {
                return BadRequest(new { success = false, message = "Start date cannot be after the end date." });
            }

            var reservedSiteIds = _unitOfWork.Reservation
                .List(r => (r.StartDate <= endDate && r.EndDate >= startDate))
                .Select(r => r.SiteId)
                .Distinct()
                .ToList();

            var availableSites = _unitOfWork.Site
                .List()
                .Where(s => !reservedSiteIds.Contains(s.Id))
                .Select(s => new { s.Id, s.Name })
                .ToList();

            return Json(availableSites);
        }


        [HttpGet("SitePrice")]
        public IActionResult GetSitePrice(int siteId)
        {
            var site = _unitOfWork.Site.Get(s => s.Id == siteId, includes: "SiteType");
            if (site == null || site.SiteType == null)
            {
                return NotFound();
            }

            return Ok(new { price = site.SiteType.Price });
        }
    }
}