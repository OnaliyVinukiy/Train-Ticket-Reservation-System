using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Services;


namespace TrainTicket.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{

    private readonly BookingService service;


    public BookingController(BookingService service)
    {
        this.service = service;
    }


    [HttpGet]
    public IActionResult GetBookings()
    {
        return Ok(service.GetAllBookings());
    }


    [HttpGet("{id}")]
    public IActionResult GetBooking(int id)
    {
        var booking = service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound();
        }


        return Ok(booking);
    }



    [HttpPost]
    public IActionResult CreateBooking(Booking booking)
    {
        var created = service.CreateBooking(booking);

        return Ok(created);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateBooking(
        int id,
        Booking booking)
    {

        if (id != booking.Id)
        {
            return BadRequest();
        }


        service.UpdateBooking(booking);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
        service.DeleteBooking(id);

        return NoContent();
    }

}