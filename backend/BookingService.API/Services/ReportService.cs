using Microsoft.EntityFrameworkCore;
using TrainTicket.API.Data;
using TrainTicket.API.Models;
using TrainTicket.API.Models.DTOs;

namespace TrainTicket.API.Services;

public class ReportService
{
    private readonly AppDbContext context;

    public ReportService(AppDbContext context)
    {
        this.context = context;
    }

    // Weekly Calendar View

    public List<WeeklyReportDto> GetWeeklyReport(DateTime startDate)
    {
        List<WeeklyReportDto> report = new();

        for (int i = 0; i < 7; i++)
        {
            DateTime current = startDate.Date.AddDays(i);

            var bookings = context.Bookings
                .Include(x => x.Route)
                .Include(x => x.Schedule)
                .Include(x => x.SpecialRequests)
                .Where(x => x.Schedule.TravelDate.Date == current)
                .ToList();

            WeeklyReportDto day = new()
            {
                Date = current,
                Day = current.DayOfWeek.ToString()
            };

            foreach (var booking in bookings)
            {
                day.Bookings.Add(new WeeklyBookingDto
                {
                    BookingId = booking.Id,
                    Route = booking.Route.DepartureStation +
                            " → " +
                            booking.Route.DestinationStation,

                    SeatNumber = booking.SeatNumber,
                    TicketPrice = booking.TicketPrice,

                    SpecialRequests = booking.SpecialRequests
                        .Select(x => x.Description)
                        .ToList()
                });
            }

            report.Add(day);
        }

        return report;
    }

    // Booking report with filters

    public List<Booking> GetBookingReport(
        DateTime fromDate,
        DateTime toDate,
        string? route,
        BookingType? bookingType)
    {
        var query = context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Include(x => x.SpecialRequests)
            .AsQueryable();

        query = query.Where(x =>
            x.Schedule.TravelDate >= fromDate &&
            x.Schedule.TravelDate <= toDate);

        if (!string.IsNullOrWhiteSpace(route))
        {
            query = query.Where(x =>
                x.Route.DepartureStation.Contains(route) ||
                x.Route.DestinationStation.Contains(route));
        }

        if (bookingType != null)
        {
            query = query.Where(x =>
                x.BookingType == bookingType);
        }

        return query.ToList();
    }

    // Route frequency

    public Dictionary<string, int> GetRouteFrequency(
        DateTime fromDate,
        DateTime toDate)
    {
        return context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Where(x =>
                x.Schedule.TravelDate >= fromDate &&
                x.Schedule.TravelDate <= toDate)
            .GroupBy(x =>
                x.Route.DepartureStation +
                " → " +
                x.Route.DestinationStation)
            .ToDictionary(
                x => x.Key,
                x => x.Count());
    }

    // Total expenditure

    public decimal GetTotalExpenditure(
        DateTime fromDate,
        DateTime toDate)
    {
        return context.Bookings
            .Include(x => x.Schedule)
            .Where(x =>
                x.Schedule.TravelDate >= fromDate &&
                x.Schedule.TravelDate <= toDate)
            .Sum(x => x.TicketPrice);
    }

    // Selected booking cost

    public decimal GetSelectedBookingsCost(
        List<int> bookingIds)
    {
        return context.Bookings
            .Where(x => bookingIds.Contains(x.Id))
            .Sum(x => x.TicketPrice);
    }

    // Dashboard summary

    public WeeklyReportSummaryDto GetWeeklySummary(
        DateTime startDate)
    {
        DateTime endDate = startDate.AddDays(6);

        var bookings = context.Bookings
            .Include(x => x.Route)
            .Include(x => x.Schedule)
            .Include(x => x.SpecialRequests)
            .Where(x =>
                x.Schedule.TravelDate.Date >= startDate.Date &&
                x.Schedule.TravelDate.Date <= endDate.Date)
            .ToList();

        return new WeeklyReportSummaryDto
        {
            TotalBookings = bookings.Count,

            TotalTicketCost = bookings.Sum(x => x.TicketPrice),

            TotalSpecialRequests = bookings.Sum(x =>
                x.SpecialRequests.Count),

            MostPopularRoute = bookings
                .GroupBy(x =>
                    x.Route.DepartureStation +
                    " → " +
                    x.Route.DestinationStation)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .FirstOrDefault() ?? "No Bookings"
        };
    }
}