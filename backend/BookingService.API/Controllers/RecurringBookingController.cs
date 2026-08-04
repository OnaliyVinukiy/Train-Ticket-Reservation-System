using Microsoft.AspNetCore.Mvc;
using BookingService.API.Models;
using BookingService.API.Services;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecurringBookingController : ControllerBase
{
    private readonly RecurringBookingService service;


    public RecurringBookingController(
        RecurringBookingService service)
    {
        this.service = service;
    }


    [HttpPost("generate")]
    public async Task<IActionResult> Generate(
        RecurringBooking booking)
    {
        var bookings =
            await service.GenerateRecurringBookings(booking);


        return Ok(new
        {
            message = "Recurring bookings generated successfully",
            count = bookings.Count,
            bookings
        });
    }
}