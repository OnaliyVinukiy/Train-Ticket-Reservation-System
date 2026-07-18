using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecurringBookingController : ControllerBase
{
    private readonly RecurringBookingService service;

    public RecurringBookingController(RecurringBookingService service)
    {
        this.service = service;
    }

    [HttpPost("generate")]
    public IActionResult Generate(RecurringBooking booking)
    {
        var bookings = service.GenerateRecurringBookings(booking);
        return Ok(new
        {
            message = "Recurring bookings generated successfully",
            count = bookings.Count,
            bookings
        });
    }
}