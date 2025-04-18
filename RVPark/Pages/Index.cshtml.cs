using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

public class IndexModel : PageModel
{
    private readonly UnitOfWork _unitOfWork;

    public IndexModel(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<Site> AvailableSites { get; set; }

    public async Task OnGetAsync()
    {
        var today = DateTime.Today;

        var allSites = await _unitOfWork.Site.ListAsync(predicate: _ => true, includes: "SiteType");
        var reservations = await _unitOfWork.Reservation
            .ListAsync(r => r.StartDate <= today && r.EndDate >= today);

        var reservedIds = reservations.Select(r => r.SiteId).ToHashSet();

        AvailableSites = allSites.Where(site => !reservedIds.Contains(site.Id)).ToList();
    }
}