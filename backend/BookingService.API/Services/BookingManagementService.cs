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
        await ValidateBooking(booking);
        return await repository.CreateBooking(booking);
    }

    public async Task UpdateBooking(Booking booking)
    {
        await ValidateBooking(booking);
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

    private async Task ValidateBooking(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.BookingReference))
            throw new ArgumentException("Booking reference is required.");

        if (string.IsNullOrWhiteSpace(booking.SeatNumber))
            throw new ArgumentException("Seat number is required.");

        if (booking.TicketPrice <= 0)
            throw new ArgumentException("Ticket price must be greater than zero.");

        var bookings = await repository.GetAllBookings();

        if (bookings.Any(b =>
            b.BookingReference == booking.BookingReference &&
            b.Id != booking.Id))
        {
            throw new ArgumentException("Booking reference already exists.");
        }

        if (bookings.Any(b =>
            b.ScheduleId == booking.ScheduleId &&
            b.SeatNumber == booking.SeatNumber &&
            b.Id != booking.Id))
        {
            throw new ArgumentException("Seat already booked for this schedule.");
        }

        if (booking.Schedule != null &&
            booking.Schedule.TravelDate.Date < DateTime.Today)
        {
            throw new ArgumentException("Travel date cannot be in the past.");
        }
    }
}