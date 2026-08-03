using BookingService.API.Models;
using BookingService.API.Repositories;

namespace BookingService.API.Services;

public class BookingManagementService
{
    private readonly BookingRepository repository;

    public BookingManagementService(
        BookingRepository repository)
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


    public List<Booking> SearchBookings(
        string? date,
        string? route,
        string? reference)
    {
        var bookings = repository.GetAllBookings();


        if (!string.IsNullOrWhiteSpace(date))
        {
            bookings = bookings
                .Where(b =>
                    b.Schedule.TravelDate
                        .ToString("yyyy-MM-dd")
                        .Contains(date))
                .ToList();
        }


        if (!string.IsNullOrWhiteSpace(route))
        {
            bookings = bookings
                .Where(b =>
                    b.Route.DepartureStation.Contains(route, StringComparison.OrdinalIgnoreCase)
                    ||
                    b.Route.DestinationStation.Contains(route, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        if (!string.IsNullOrWhiteSpace(reference))
        {
            bookings = bookings
                .Where(b =>
                    b.BookingReference.Contains(reference, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }


        return bookings;
    }
}