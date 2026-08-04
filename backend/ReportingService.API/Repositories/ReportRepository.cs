using Microsoft.EntityFrameworkCore;
using ReportingService.API.Data;
using ReportingService.API.Models;

namespace ReportingService.API.Repositories;

public class ReportRepository
{
    private readonly AppDbContext context;

    public ReportRepository(AppDbContext context)
    {
        this.context = context;
    }


    public IQueryable<Booking> GetBookings()
    {
        return context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Include(x => x.SpecialRequests);
    }


    public List<Booking> GetBookingsByDate(
        DateTime date)
    {
        return context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Include(x => x.SpecialRequests)
            .Where(x =>
                x.Schedule.TravelDate.Date == date.Date)
            .ToList();
    }


    public List<Booking> GetBookingsBetweenDates(
        DateTime fromDate,
        DateTime toDate)
    {
        return context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Include(x => x.SpecialRequests)
            .Where(x =>
                x.Schedule.TravelDate >= fromDate &&
                x.Schedule.TravelDate <= toDate)
            .ToList();
    }


    public List<Booking> GetBookingsByIds(
        List<int> ids)
    {
        return context.Bookings
            .Where(x => ids.Contains(x.Id))
            .ToList();
    }
}