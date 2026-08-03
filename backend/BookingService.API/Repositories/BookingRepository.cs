using Microsoft.EntityFrameworkCore;
using BookingService.API.Data;
using BookingService.API.Models;

namespace BookingService.API.Repositories;

public class BookingRepository
{
    private readonly AppDbContext context;


    public BookingRepository(
        AppDbContext context)
    {
        this.context = context;
    }


    public List<Booking> GetAllBookings()
    {
        return context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .ToList();
    }


    public Booking? GetBooking(int id)
    {
        return context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .FirstOrDefault(b => b.Id == id);
    }


    public Booking CreateBooking(
        Booking booking)
    {
        context.Bookings.Add(booking);

        context.SaveChanges();

        return booking;
    }


    public void UpdateBooking(Booking booking)
    {
        var existingBooking = context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .FirstOrDefault(b => b.Id == booking.Id);


        if (existingBooking == null)
            return;

        // Update booking details
        existingBooking.SeatNumber =
            booking.SeatNumber;

        existingBooking.TicketPrice =
            booking.TicketPrice;

        existingBooking.BookingType =
            booking.BookingType;


        // Update route
        existingBooking.Route.DepartureStation =
            booking.Route.DepartureStation;

        existingBooking.Route.DestinationStation =
            booking.Route.DestinationStation;



        // Update schedule
        existingBooking.Schedule.TravelDate =
            booking.Schedule.TravelDate;

        existingBooking.Schedule.DepartureTime =
            booking.Schedule.DepartureTime;

        existingBooking.Schedule.ArrivalTime =
            booking.Schedule.ArrivalTime;


        // -----------------------------
        // Update Special Requests
        // -----------------------------

        // Remove unchecked requests
        var removedRequests =
            existingBooking.SpecialRequests
            .Where(existing =>
                !booking.SpecialRequests
                .Any(updated =>
                    updated.Id == existing.Id))
            .ToList();


        foreach (var request in removedRequests)
        {
            context.SpecialRequests.Remove(request);
        }


        // Update existing and add new requests
        foreach (var request in booking.SpecialRequests)
        {
            var existingRequest =
                existingBooking.SpecialRequests
                .FirstOrDefault(x => x.Id == request.Id);


            if (existingRequest != null)
            {
                // Existing request edited
                existingRequest.Description =
                    request.Description;
            }
            else
            {
                // New request added
                existingBooking.SpecialRequests.Add(
                    new SpecialRequest
                    {
                        Description = request.Description
                    });
            }
        }

        context.SaveChanges();
    }

    public void DeleteBooking(
        int id)
    {
        var booking =
            context.Bookings
            .FirstOrDefault(x => x.Id == id);


        if (booking != null)
        {
            context.Bookings.Remove(booking);
            context.SaveChanges();
        }
    }

    public bool BookingExists(
        DateTime date,
        string departure,
        string destination)
    {
        return context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Any(b =>
                b.Schedule.TravelDate.Date == date.Date
                &&
                b.Route.DepartureStation == departure
                &&
                b.Route.DestinationStation == destination
            );
    }
}