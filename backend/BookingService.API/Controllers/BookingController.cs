using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Services;


namespace TrainTicket.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{

    private readonly BookingService service;


    public BookingController(
        BookingService service
    )
    {
        this.service = service;
    }


    [HttpGet]
    public IActionResult GetBookings()
    {
        return Ok(
            service.GetAllBookings()
        );
    }


    [HttpGet("{id}")]
    public IActionResult GetBooking(
        int id
    )
    {
        var booking = service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound(
                new
                {
                    message = "Booking not found"
                }
            );
        }


        return Ok(booking);
    }



    [HttpPost]
    public IActionResult CreateBooking(
        Booking booking
    )
    {

        var created = service.CreateBooking(booking);


        return CreatedAtAction(
            nameof(GetBooking),
            new
            {
                id = created.Id
            },
            created
        );

    }


    [HttpPut("{id}")]
    public IActionResult UpdateBooking(
        int id,
        Booking booking
    )
    {

        if (id != booking.Id)
        {
            return BadRequest(
                new
                {
                    message = "Booking ID mismatch"
                }
            );
        }


        service.UpdateBooking(booking);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(
        int id
    )
    {

        var booking = service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound(
                new
                {
                    message = "Booking not found"
                }
            );
        }


        service.DeleteBooking(id);


        return NoContent();

    }

    [HttpGet("search")]
    public IActionResult SearchBookings(
        string? date,
        string? route,
        string? reference
    )
    {

        var bookings = service.SearchBookings(
            date,
            route,
            reference
        );


        return Ok(bookings);

    }

    [HttpGet("total-cost")]
    public IActionResult GetTotalCost()
    {

        var total = service.GetAllBookings()
            .Sum(x => x.TicketPrice);


        return Ok(
            new
            {
                totalCost = total
            }
        );

    }

}