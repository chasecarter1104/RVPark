using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IndexModel : PageModel
{
    private readonly UnitOfWork _unitOfWork;

    [BindProperty(SupportsGet = true)]
    public DateTime? FilterStartDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? FilterEndDate { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? FilterSiteTypeId { get; set; }

    public IEnumerable<SelectListItem> SiteTypeList { get; set; }


    public IndexModel(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<Site> AvailableSites { get; set; }

    public async Task OnGetAsync()
    {
        var allSites = await _unitOfWork.Site.ListAsync(predicate: _ => true, includes: "SiteType");

        var siteTypes = await _unitOfWork.SiteType.ListAsync();
        SiteTypeList = siteTypes.Select(st => new SelectListItem
        {
            Value = st.Id.ToString(),
            Text = st.Name
        });

        AvailableSites = (List<Site>)allSites;

        if (FilterSiteTypeId.HasValue)
        {
            AvailableSites = AvailableSites
                .Where(s => s.SiteTypeId == FilterSiteTypeId.Value)
                .ToList();
        }

        if (FilterStartDate.HasValue && FilterEndDate.HasValue && FilterStartDate < FilterEndDate)
        {
            var reservations = await _unitOfWork.Reservation.ListAsync(r =>
                r.StartDate < FilterEndDate && r.EndDate > FilterStartDate);

            var reservedIds = reservations.Select(r => r.SiteId).ToHashSet();

            AvailableSites = AvailableSites
                .Where(site => !reservedIds.Contains(site.Id))
                .ToList();
        }
        else
        {
            // Default to today's availability if no filter provided
            var today = DateTime.Today;
            var reservations = await _unitOfWork.Reservation
                .ListAsync(r => r.StartDate <= today && r.EndDate >= today);

            var reservedIds = reservations.Select(r => r.SiteId).ToHashSet();

            AvailableSites = AvailableSites
                .Where(site => !reservedIds.Contains(site.Id))
                .ToList();
        }

    }

}