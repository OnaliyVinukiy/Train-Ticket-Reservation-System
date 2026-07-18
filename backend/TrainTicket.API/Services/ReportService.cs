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
        var weeklyReport = new List<WeeklyReportDto>();

        for (int i = 0; i < 7; i++)
        {
            DateTime currentDate = startDate.Date.AddDays(i);

            var bookings = context.Bookings
                .Include(b => b.Route)
                .Include(b => b.Schedule)
                .Include(b => b.SpecialRequests)
                .Where(b => b.Schedule.TravelDate.Date == currentDate.Date)
                .ToList();

            var dailyReport = new WeeklyReportDto
            {
                Date = currentDate,
                Day = currentDate.DayOfWeek.ToString()
            };

            foreach (var booking in bookings)
            {
                dailyReport.Bookings.Add(new WeeklyBookingDto
                {
                    BookingId = booking.Id,
                    Route = booking.Route.DepartureStation + " → " + booking.Route.DestinationStation,
                    SeatNumber = booking.SeatNumber,
                    TicketPrice = booking.TicketPrice,
                    SpecialRequests = booking.SpecialRequests.Select(x => x.Description).ToList()
                });
            }

            weeklyReport.Add(dailyReport);
        }

        return weeklyReport;
    }

    public List<Booking> GetBookingReport(DateTime fromDate, DateTime toDate, string? route, BookingType? bookingType)
    {
        var query = context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Include(b => b.SpecialRequests)
            .Where(b => b.Schedule.TravelDate >= fromDate && b.Schedule.TravelDate <= toDate)
            .AsQueryable();

        if (!string.IsNullOrEmpty(route))
        {
            query = query.Where(b =>
                b.Route.DepartureStation.Contains(route) ||
                b.Route.DestinationStation.Contains(route));
        }

        if (bookingType != null)
        {
            query = query.Where(b => b.BookingType == bookingType);
        }

        return query.ToList();
    }


    public Dictionary<string, int> GetRouteFrequency(DateTime fromDate, DateTime toDate)
    {
        return context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Where(b => b.Schedule.TravelDate >= fromDate && b.Schedule.TravelDate <= toDate)
            .GroupBy(b => b.Route.DepartureStation + " → " + b.Route.DestinationStation)
            .ToDictionary(x => x.Key, x => x.Count());
    }

  
    public decimal GetTotalExpenditure(DateTime fromDate, DateTime toDate)
    {
        return context.Bookings
            .Include(b => b.Schedule)
            .Where(b => b.Schedule.TravelDate >= fromDate && b.Schedule.TravelDate <= toDate)
            .Sum(b => b.TicketPrice);
    }


    public decimal GetSelectedBookingsCost(List<int> bookingIds)
    {
        return context.Bookings
            .Where(b => bookingIds.Contains(b.Id))
            .Sum(b => b.TicketPrice);
    }
}