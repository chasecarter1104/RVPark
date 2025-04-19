using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Stripe;

namespace RVPark.Pages.Customer.Reservations
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;

        [BindProperty]
        public Reservation Reservation { get; set; }

        // Creating these for a dropdown list
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
        public IEnumerable<SelectListItem> FeeList { get; set; }

        public float TotalCost { get; set; }

        public User User { get; set; }

        // Constructor injection
        public UpsertModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet(int? id)
        {
            var sites = _unitOfWork.Site.List();
            var users = _unitOfWork.User.List();
            var fees = _unitOfWork.Fee.List();

            if (id != null)
            {
                Reservation = _unitOfWork.Reservation.Get(u => u.Id == id, true);
            }

            if (Reservation == null) // New upsert
            {
                Reservation = new Reservation();
            }

            SiteList = sites.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
            UserList = users.Select(u => new SelectListItem { Text = u.FullName, Value = u.Id.ToString() }).ToList();
            FeeList = fees.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

            User = users.First(c => c.Email == HttpContext.User.Identity?.Name);
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
                _unitOfWork.Reservation.Update(Reservation);
            }

            // Save changes to the database
            _unitOfWork.Commit();

            // Redirect
            return RedirectToPage();
        }
    }
}
