using TrainTicket.API.Models;
using TrainTicket.API.Repositories;

namespace TrainTicket.API.Services;

public class BookingService
{
    private readonly BookingRepository repository;

    public BookingService(BookingRepository repository)
    {
        this.repository = repository;
    }


    public List<Booking> GetAllBookings()
    {
        return repository.GetAllBookings();
    }


    public Booking? GetBookingById(int id)
    {
        return repository.GetBooking(id);
    }


    public Booking CreateBooking(Booking booking)
    {
        return repository.CreateBooking(booking);
    }


    public void UpdateBooking(Booking booking)
    {
        repository.UpdateBooking(booking);
    }


    public void DeleteBooking(int id)
    {
        repository.DeleteBooking(id);
    }


    public List<Booking> SearchBookings(string? date, string? route, string? reference)
    {
        var bookings = repository.GetAllBookings().AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            bookings = bookings.Where(b =>
                b.Schedule.TravelDate.ToString("yyyy-MM-dd").Contains(date)
            );
        }

        if (!string.IsNullOrEmpty(route))
        {
            bookings = bookings.Where(b =>
                b.Route.DepartureStation.Contains(route, StringComparison.OrdinalIgnoreCase) ||
                b.Route.DestinationStation.Contains(route, StringComparison.OrdinalIgnoreCase)
            );
        }

        if (!string.IsNullOrEmpty(reference))
        {
            bookings = bookings.Where(b =>
                b.BookingReference.Contains(reference, StringComparison.OrdinalIgnoreCase)
            );
        }

        return bookings.ToList();
    }

    public decimal GetTotalCost()
    {
        return repository.GetAllBookings().Sum(b => b.TicketPrice);
    }
}