using Microsoft.EntityFrameworkCore;
using BookingService.API.Data;
using BookingService.API.Models;

namespace BookingService.API.Repositories;

public class BookingRepository
{
    private readonly AppDbContext context;

    public BookingRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .ToListAsync();
    }

    public async Task<Booking?> GetBooking(int id)
    {
        return await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Booking> CreateBooking(Booking booking)
    {
        await context.Bookings.AddAsync(booking);
        await context.SaveChangesAsync();

        return booking;
    }

    public async Task UpdateBooking(Booking booking)
    {
        var existingBooking = await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .FirstOrDefaultAsync(b => b.Id == booking.Id);

        if (existingBooking == null)
            return;

        // Update booking details
        existingBooking.SeatNumber = booking.SeatNumber;
        existingBooking.TicketPrice = booking.TicketPrice;
        existingBooking.BookingType = booking.BookingType;

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

        // Remove deleted requests
        var removedRequests = existingBooking.SpecialRequests
            .Where(existing =>
                !booking.SpecialRequests.Any(updated => updated.Id == existing.Id))
            .ToList();

        foreach (var request in removedRequests)
        {
            context.SpecialRequests.Remove(request);
        }

        // Update existing requests and add new ones
        foreach (var request in booking.SpecialRequests)
        {
            var existingRequest = existingBooking.SpecialRequests
                .FirstOrDefault(x => x.Id == request.Id);

            if (existingRequest != null)
            {
                existingRequest.Description = request.Description;
            }
            else
            {
                existingBooking.SpecialRequests.Add(
                    new SpecialRequest
                    {
                        Description = request.Description
                    });
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteBooking(int id)
    {
        var booking = await context.Bookings
            .FirstOrDefaultAsync(x => x.Id == id);

        if (booking != null)
        {
            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> BookingExists(
        DateTime date,
        string departure,
        string destination)
    {
        return await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .AnyAsync(b =>
                b.Schedule.TravelDate.Date == date.Date &&
                b.Route.DepartureStation == departure &&
                b.Route.DestinationStation == destination);
    }
}