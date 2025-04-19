using ApplicationCore.Models;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;

namespace RVPark.Pages.Admin.Reservations
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public Reservation Reservation { get; set; }

        // Creating these for a dropdown list
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }

        // Constructor injection
        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int? id)
        {
            var sites = _unitOfWork.Site.List();
            var users = _unitOfWork.User.List();

            if (id != null)
            {
                // Get the reservation if id is provided
                Reservation = _unitOfWork.Reservation.Get(u => u.Id == id, true);
            }

            if (Reservation == null) // New reservation
            {
                Reservation = new Reservation();
            }

            SiteList = sites.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            UserList = users.Select(u => new SelectListItem { Text = u.FullName, Value = u.Id.ToString() }).ToList();
        }

        public IActionResult OnPost(int? id)
        {
            if (Reservation.Id == 0)
            {
                // Add new reservation to the database
                _unitOfWork.Reservation.Add(Reservation);
            }
            else
            {
                // Update existing reservation
                var objFromDb = _unitOfWork.Reservation.Get(r => r.Id == Reservation.Id, true);
                if (objFromDb != null)
                {
                    // Update the reservation with new values
                    _unitOfWork.Reservation.Update(Reservation);
                }
                else
                {
                    // Handle case where reservation is not found (e.g., return error)
                    return NotFound();
                }
            }

            // Save changes to the database
            _unitOfWork.Commit();

            // Redirect
            return RedirectToPage("./Index");
        }
    }
}