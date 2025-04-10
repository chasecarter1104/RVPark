using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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

            return File(dummyPdf, "application/pdf", "test.pdf");
            /*
            var reservations = await _unitOfWork.Reservation
                .ListAsync(r => r.StartDate >= startDate && r.StartDate <= endDate);

            byte[] pdfBytes = GeneratePdfReport(reservations);

            return File(pdfBytes, "application/pdf", $"ReservationsReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
            */
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
