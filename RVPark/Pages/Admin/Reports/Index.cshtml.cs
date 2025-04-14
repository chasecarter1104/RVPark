using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;

public class ReportModel : PageModel
{
    private readonly UnitOfWork _unitOfWork;

    public ReportModel(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // The Razor Page is mapping to Model.Reservations 
    // And the Model is ReportModel
    public List<ReservationReportViewModel> Reservations { get; set; } = new();


    public void OnGet()
    {
        var today = DateTime.Today;

        // Use includes for eager loading Site and SiteType
        // Eager loading means to also load the related entities when you load the main
        // It is detailed with the "includes"
        // _ is a placeholder for the current item, and the => is a lambda which includes all of them.
        // You could also use predicate: null as well, or just null, but I'm writing this way to make sense of it all.
        //var reservations = _unitOfWork.Reservation.List(null, null, "Site,Site.SiteType");
        var reservations = _unitOfWork.Reservation.List(
            predicate: _ => true,
            includes: "Site,Site.SiteType" // Goes through the foreign key to get to Site and then another foreign key of Site to SiteType
        );


        // reservations are an IEnumerable<T> because the _unitOfWork.Reservation.List() returns an IEnumerable<T>
        // Enumerable is the helper class for IEnumerable, it is a static class in System.Linq (what we're using for Entity Framework).
        // Enumerable provides extension methods for IEnumerable<T>
        // The "I" means interface. An Interface defines methods but doesn't implement them.
        // Enumerable is the class that implements the methods for IEnumerable
        // Helper classes provide methods that "help" with utility or convenience.
        // Some of the helper methods of Enumerable are Where, Select, and FirstOrDefault
        // Example:
        // IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        // var evenNumbers = numbers.Where(n => n % 2 == 0);
        Reservations = reservations.Select(r => new ReservationReportViewModel
        {
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            SiteName = r.Site?.Name ?? "No Site",
            Status = GetStatus(r.StartDate, r.EndDate, today),
            TotalPrice = CalculateTotalPrice(r.StartDate, r.EndDate, r.Site?.SiteType?.Price ?? 0)
        }).ToList();
    }

    private string GetStatus(DateTime start, DateTime end, DateTime today)
    {
        if (start > today) return "Upcoming";
        if (start <= today && end >= today) return "In Progress";
        return "Completed";
    }

    private decimal CalculateTotalPrice(DateTime start, DateTime end, int pricePerDay)
    {
        int days = (end - start).Days;
        if (days <= 0) days = 1;
        return pricePerDay * days;
    }

    public class ReservationReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string SiteName { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
    }
}




/*
namespace RVPark.Pages.Admin.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // OnPost is what is called, but it can include other words after OnPost and it will still call it.
        public async Task<IActionResult> OnPostGenerateReport(DateTime startDate, DateTime endDate)
        {
            // Test line to confirm it's hit
            _logger.LogInformation("Start date: {start}, End date: {end}", startDate, endDate);
            System.Diagnostics.Debug.WriteLine("Handler hit: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("OnPostGenerateReport was triggered!");

            // Just return test PDF data to isolate the issue
            byte[] dummyPdf = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // "%PDF" header — not a real PDF, but triggers a download

            // Set Content-Disposition header to indicate download
            Response.Headers.Add("Content-Disposition", "attachment; filename=test.pdf");

            //return File(dummyPdf, "application/pdf", "test.pdf");
            
            //var reservations = await _unitOfWork.Reservation
            //    .ListAsync(r => r.StartDate >= startDate && r.StartDate <= endDate);

            //byte[] pdfBytes = GeneratePdfReport(reservations);

            //return File(pdfBytes, "application/pdf", $"ReservationsReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
            
        }

        private byte[] GeneratePdfReport(IEnumerable<Reservation> reservations)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Reservation Report").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100); // Start Date
                            columns.ConstantColumn(100); // End Date
                            columns.RelativeColumn();    // Site
                            columns.RelativeColumn();    // User
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Start").Bold();
                            header.Cell().Text("End").Bold();
                            header.Cell().Text("Site").Bold();
                            header.Cell().Text("User").Bold();
                        });

                        foreach (var r in reservations)
                        {
                            table.Cell().Text(r.StartDate.ToString("MM/dd/yyyy"));
                            table.Cell().Text(r.EndDate.ToString("MM/dd/yyyy"));
                            table.Cell().Text(r.Site?.Name ?? "N/A");
                            table.Cell().Text(r.User?.UserName ?? "N/A");
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated on ");
                        x.Span(DateTime.Now.ToString("f")).SemiBold();
                    });

                });
            }).GeneratePdf();
        }
    }
}
*/