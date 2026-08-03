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


    public void UpdateBooking(
        Booking booking)
    {
        var existing =
            context.Bookings
            .FirstOrDefault(x => x.Id == booking.Id);


        if (existing == null)
            return;


        existing.SeatNumber =
            booking.SeatNumber;


        existing.TicketPrice =
            booking.TicketPrice;


        existing.BookingType =
            booking.BookingType;


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