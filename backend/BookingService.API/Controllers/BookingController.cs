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
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await service.GetAllBookings();

        return Ok(
            bookings.Select(BookingMapper.ToDto)
        );
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await service.GetBookingById(id);


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
    public async Task<IActionResult> CreateBooking(
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
                await service.GenerateRecurringBookings(recurringBooking);


            return Ok(new
            {
                message = "Recurring bookings created successfully",
                count = generated.Count,
                bookings = generated
            });
        }


        var created =
            await service.CreateBooking(booking);


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
    public async Task<IActionResult> UpdateBooking(
        int id,
        BookingDto dto)
    {
        var existing =
            await service.GetBookingById(id);


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


        await service.UpdateBooking(booking);


        return NoContent();
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(
        int id)
    {
        var booking =
            await service.GetBookingById(id);


        if (booking == null)
        {
            return NotFound(new
            {
                message = "Booking not found"
            });
        }


        await service.DeleteBooking(id);


        return NoContent();
    }



    [HttpGet("search")]
    public async Task<IActionResult> SearchBookings(
        string? date,
        string? route,
        string? reference)
    {
        var bookings =
            await service.SearchBookings(
                date,
                route,
                reference
            );


        return Ok(
            bookings.Select(BookingMapper.ToDto)
        );
    }
}