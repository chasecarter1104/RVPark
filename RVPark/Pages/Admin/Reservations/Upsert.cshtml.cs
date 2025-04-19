using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Infrastructure.Data;

namespace RVPark.Pages.Admin.Reservations
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public Reservation Reservation { get; set; }

        [BindProperty]
        public List<int> SelectedFeeIds { get; set; } = new();

        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
        public IEnumerable<SelectListItem> FeeList { get; set; }

        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int? id)
        {
            var sites = _unitOfWork.Site.List();
            var users = _unitOfWork.User.List();
            var fees = _unitOfWork.Fee.List();

            SiteList = sites.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            UserList = users.Select(u => new SelectListItem { Text = u.FullName, Value = u.Id.ToString() }).ToList();
            FeeList = fees.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = $"{f.Name} - ${f.FeeAmount}"
            }).ToList();

            if (id != null)
            {
                Reservation = _unitOfWork.Reservation.Get(r => r.Id == id, includes: "Fees");
                if (Reservation != null)
                {
                    SelectedFeeIds = Reservation.Fees.Select(f => f.Id).ToList();
                }
            }

            if (Reservation == null)
            {
                Reservation = new Reservation();
            }
        }

        public IActionResult OnPost()
        {


            // Get the selected Fee entities
            var selectedFees = _unitOfWork.Fee.List().Where(f => SelectedFeeIds.Contains(f.Id)).ToList();
            Reservation.Fees = selectedFees;

            if (Reservation.Id == 0)
            {
                _unitOfWork.Reservation.Add(Reservation);
            }
            else
            {
                var objFromDb = _unitOfWork.Reservation.Get(r => r.Id == Reservation.Id, includes: "Fees");
                if (objFromDb == null)
                {
                    return NotFound();
                }

                // Update fields
                objFromDb.StartDate = Reservation.StartDate;
                objFromDb.EndDate = Reservation.EndDate;
                objFromDb.SiteId = Reservation.SiteId;
                objFromDb.UserId = Reservation.UserId;

                // Update fees
                objFromDb.Fees.Clear();
                foreach (var fee in selectedFees)
                {
                    objFromDb.Fees.Add(fee);
                }

                _unitOfWork.Reservation.Update(objFromDb);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
