using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Repositories;


namespace TrainTicket.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{

    private readonly IRepository<Booking> repository;


    public BookingController(
        IRepository<Booking> repository)
    {
        this.repository = repository;
    }



    [HttpGet]
    public IActionResult GetBookings()
    {
        return Ok(repository.GetAll());
    }



    [HttpPost]
    public IActionResult CreateBooking(
        Booking booking)
    {
        repository.Add(booking);

        return Ok(booking);
    }



    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(int id)
    {
        repository.Delete(id);

        return NoContent();
    }

}