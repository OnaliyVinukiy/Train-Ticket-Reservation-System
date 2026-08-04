using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ReportingService.API.Models;
using ReportingService.API.Services;

namespace ReportingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService service;
    private readonly ReportExportWorker worker;

    public ReportController(ReportService service, ReportExportWorker worker)
    {
        this.service = service;
        this.worker = worker;
    }

    [HttpGet("weekly")]
    public IActionResult GetWeeklyReport(DateTime startDate)
    {
        return Ok(service.GetWeeklyReport(startDate));
    }

    [HttpGet("summary")]
    public IActionResult GetSummary(DateTime startDate)
    {
        return Ok(service.GetWeeklySummary(startDate));
    }

    [HttpGet("bookings")]
    public IActionResult GetBookingReport(
        DateTime fromDate,
        DateTime toDate,
        string? route,
        string? bookingType)
    {
        BookingType? type = null;

        if (!string.IsNullOrWhiteSpace(bookingType))
        {
            Enum.TryParse(bookingType, true, out BookingType parsed);
            type = parsed;
        }

        return Ok(
            service.GetBookingReport(
                fromDate,
                toDate,
                route,
                type
            )
        );
    }

    [HttpGet("route-frequency")]
    public IActionResult GetRouteFrequency(
        DateTime fromDate,
        DateTime toDate)
    {
        return Ok(
            service.GetRouteFrequency(
                fromDate,
                toDate
            )
        );
    }

    [HttpGet("total-expenditure")]
    public IActionResult GetTotalExpenditure(
        DateTime fromDate,
        DateTime toDate)
    {
        return Ok(
            service.GetTotalExpenditure(
                fromDate,
                toDate
            )
        );
    }

    [HttpPost("selected-cost")]
    public IActionResult GetSelectedBookingsCost(
        [FromBody] List<int> bookingIds)
    {
        return Ok(
            service.GetSelectedBookingsCost(
                bookingIds
            )
        );
    }


    [HttpPost("export/start")]
    public IActionResult StartExport()
    {

        var jobId =
            worker.QueueExport();


        return Ok(new
        {
            Message =
            "Report generation started",

            JobId =
            jobId
        });

    }


    [HttpGet("export/status/{id}")]
    public IActionResult ExportStatus(
    Guid id)
    {

        var job =
            worker.GetStatus(id);

        if (job == null)
            return NotFound();

        return Ok(job);

    }

    [HttpGet("configuration")]
    public IActionResult GetConfiguration()
    {
        return Ok(
            service.GetReportConfiguration()
        );
    }
}