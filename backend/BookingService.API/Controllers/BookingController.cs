using Microsoft.AspNetCore.Mvc;
using BookingService.API.DTOs;
using BookingService.API.Helpers;
using BookingService.API.Services;
using BookingService.API.Models;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingManagementService service;


    public BookingController(
        BookingManagementService service)
    {
        this.service = service;
    }



    [HttpGet]
    public IActionResult GetBookings()
    {
        var bookings = service.GetAllBookings();

        return Ok(
            bookings.Select(BookingMapper.ToDto)
        );
    }



    [HttpGet("{id}")]
    public IActionResult GetBooking(int id)
    {
        var booking = service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound(new
            {
                message = "Booking not found"
            });
        }


        return Ok(
            BookingMapper.ToDto(booking)
        );
    }


    [HttpPost]
    public IActionResult CreateBooking(
    BookingDto dto)
    {
        var booking =
            BookingMapper.ToModel(dto);


        if (booking.BookingType == BookingType.Recurring)
        {
            var recurringBooking = new RecurringBooking
            {
                SeatNumber = booking.SeatNumber,
                TicketPrice = booking.TicketPrice,
                RecurrencePattern = booking.RecurrencePattern,
                RecurrenceEndDate = booking.RecurrenceEndDate,

                Route = booking.Route,

                Schedule = booking.Schedule,

                SpecialRequests = booking.SpecialRequests
            };


            var generated =
                service.GenerateRecurringBookings(recurringBooking);


            return Ok(new
            {
                message = "Recurring bookings created successfully",
                count = generated.Count,
                bookings = generated
            });
        }



        var created =
            service.CreateBooking(booking);


        return CreatedAtAction(
            nameof(GetBooking),
            new
            {
                id = created.Id
            },
            BookingMapper.ToDto(created)
        );
    }



    [HttpPut("{id}")]
    public IActionResult UpdateBooking(
        int id,
        BookingDto dto)
    {
        var existing =
            service.GetBookingById(id);


        if (existing == null)
        {
            return NotFound(new
            {
                message = "Booking not found"
            });
        }


        var booking =
            BookingMapper.ToModel(dto);


        booking.Id = id;


        service.UpdateBooking(booking);


        return NoContent();
    }





    [HttpDelete("{id}")]
    public IActionResult DeleteBooking(
        int id)
    {
        var booking =
            service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound(new
            {
                message = "Booking not found"
            });
        }


        service.DeleteBooking(id);


        return NoContent();
    }





    [HttpGet("search")]
    public IActionResult SearchBookings(
        string? date,
        string? route,
        string? reference)
    {
        var bookings =
            service.SearchBookings(
                date,
                route,
                reference
            );


        return Ok(
            bookings.Select(BookingMapper.ToDto)
        );
    }
}