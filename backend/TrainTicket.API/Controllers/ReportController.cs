using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService service;
    private readonly ExportService exportService;

    public ReportController(
      ReportService service,
      ExportService exportService
  )
    {
        this.service = service;
        this.exportService = exportService;
    }

    // Weekly Calendar Report
    [HttpGet("weekly")]
    public IActionResult GetWeeklyReport(DateTime? startDate)
    {
        var date = startDate ?? DateTime.Today;
        return Ok(service.GetWeeklyReport(date));
    }

    [HttpGet("booking")]
    public IActionResult BookingReport(DateTime fromDate, DateTime toDate, string? route, BookingType? bookingType)
    {
        return Ok(service.GetBookingReport(fromDate, toDate, route, bookingType));
    }

    [HttpGet("route-frequency")]
    public IActionResult RouteFrequency(DateTime fromDate, DateTime toDate)
    {
        return Ok(service.GetRouteFrequency(fromDate, toDate));
    }


    [HttpGet("expenditure")]
    public IActionResult TotalExpenditure(DateTime fromDate, DateTime toDate)
    {
        return Ok(new { total = service.GetTotalExpenditure(fromDate, toDate) });
    }


    [HttpPost("total-cost")]
    public IActionResult TotalSelectedCost(List<int> bookingIds)
    {
        return Ok(new { total = service.GetSelectedBookingsCost(bookingIds) });
    }


    [HttpGet("export")]
    public IActionResult ExportReport(
        DateTime fromDate,
        DateTime toDate,
        string? route,
        BookingType? bookingType
    )
    {

        var bookings =
            service.GetBookingReport(
                fromDate,
                toDate,
                route,
                bookingType
            );


        var file =
            exportService.ExportBookingsToCsv(
                bookings
            );


        return File(
            file,
            "text/csv",
            "booking-report.csv"
        );

    }
}