using BookingService.API.Models;
using BookingService.API.Repositories;

namespace BookingService.API.Services;

public class BookingManagementService
{
    private readonly BookingRepository repository;
    private readonly RecurringBookingService recurringService;

    public BookingManagementService(
        BookingRepository repository,
        RecurringBookingService recurringService)
    {
        this.repository = repository;
        this.recurringService = recurringService;
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await repository.GetAllBookings();
    }

    public async Task<Booking?> GetBookingById(int id)
    {
        return await repository.GetBooking(id);
    }

    public async Task<Booking> CreateBooking(Booking booking)
    {
        return await repository.CreateBooking(booking);
    }

    public async Task UpdateBooking(Booking booking)
    {
        await repository.UpdateBooking(booking);
    }

    public async Task DeleteBooking(int id)
    {
        await repository.DeleteBooking(id);
    }

    public async Task<List<Booking>> SearchBookings(
        string? date,
        string? route,
        string? reference)
    {
        var bookings = await repository.GetAllBookings();

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

    public async Task<List<Booking>> GenerateRecurringBookings(
     RecurringBooking booking)
    {
        return await recurringService.GenerateRecurringBookings(booking);
    }
}