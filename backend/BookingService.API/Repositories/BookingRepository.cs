using Microsoft.EntityFrameworkCore;
using TrainTicket.API.Data;
using TrainTicket.API.Models;

namespace TrainTicket.API.Repositories;

public class BookingRepository
{
    private readonly AppDbContext context;

    public BookingRepository(AppDbContext context)
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

    public Booking CreateBooking(Booking booking)
    {
        context.Routes.Add(booking.Route);
        context.Schedules.Add(booking.Schedule);
        context.SaveChanges();

        booking.RouteId = booking.Route.Id;
        booking.ScheduleId = booking.Schedule.Id;

        context.Bookings.Add(booking);
        context.SaveChanges();

        return booking;
    }

    public void UpdateBooking(Booking booking)
    {
        context.Bookings.Update(booking);
        context.SaveChanges();
    }

    public void DeleteBooking(int id)
    {
        var booking = context.Bookings.FirstOrDefault(x => x.Id == id);
        if (booking != null)
        {
            context.Bookings.Remove(booking);
            context.SaveChanges();
        }
    }

    public bool BookingExists(
    DateTime date,
    string departure,
    string destination
)
{

    return context.Bookings
        .Include(b => b.Route)
        .Include(b => b.Schedule)
        .Any(
            b =>
            b.Schedule.TravelDate == date
            &&
            b.Route.DepartureStation == departure
            &&
            b.Route.DestinationStation == destination
        );

}
}