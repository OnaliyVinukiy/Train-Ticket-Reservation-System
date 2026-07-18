using CsvHelper;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService service;

    public ReportController(ReportService service)
    {
        this.service = service;
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

    [HttpGet("export")]
    public IActionResult ExportCSV(DateTime fromDate, DateTime toDate)
    {
        var bookings = service.GetBookingReport(fromDate, toDate, null, null);

        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(bookings.Select(b => new
        {
            BookingId = b.Id,
            Reference = b.BookingReference,
            Route = b.Route.DepartureStation + " → " + b.Route.DestinationStation,
            Date = b.Schedule.TravelDate.ToString("yyyy-MM-dd"),
            Departure = b.Schedule.DepartureTime,
            Arrival = b.Schedule.ArrivalTime,
            Seat = b.SeatNumber,
            Price = b.TicketPrice,
            SpecialRequests = string.Join(", ", b.SpecialRequests.Select(x => x.Description))
        }));

        writer.Flush();

        return File(memoryStream.ToArray(), "text/csv", "booking-report.csv");
    }

    [HttpGet("route-frequency")]
    public IActionResult GetRouteFrequency(
    DateTime fromDate,
    DateTime toDate
)
    {
        return Ok(
            service.GetRouteFrequency(
                fromDate,
                toDate
            )
        );
    }
}