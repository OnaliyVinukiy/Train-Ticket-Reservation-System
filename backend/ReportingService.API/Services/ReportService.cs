using ReportingService.API.Models;
using ReportingService.API.DTOs;
using ReportingService.API.Repositories;

namespace ReportingService.API.Services;

public class ReportService
{
    private readonly ReportRepository repository;

    public ReportService(
        ReportRepository repository)
    {
        this.repository = repository;
    }


    // Weekly Calendar Report
    public List<WeeklyReportDto> GetWeeklyReport(
        DateTime startDate)
    {
        List<WeeklyReportDto> report = new();


        for (int i = 0; i < 7; i++)
        {
            DateTime current =
                startDate.Date.AddDays(i);


            var bookings =
                repository.GetBookingsByDate(current);

            WeeklyReportDto day = new()
            {
                Date = current,
                Day = current.DayOfWeek.ToString()
            };

            foreach (var booking in bookings)
            {
                day.Bookings.Add(
                    new WeeklyBookingDto
                    {
                        BookingId = booking.Id,

                        Route =
                        booking.Route.DepartureStation
                        + " → " +
                        booking.Route.DestinationStation,


                        SeatNumber =
                        booking.SeatNumber,


                        TicketPrice =
                        booking.TicketPrice,


                        SpecialRequests =
                        booking.SpecialRequests
                        .Select(x => x.Description)
                        .ToList()
                    });
            }

            report.Add(day);
        }

        return report;
    }



    // Booking Report Filtering
    public List<Booking> GetBookingReport(
        DateTime fromDate,
        DateTime toDate,
        string? route,
        BookingType? bookingType)
    {

        var query =
            repository.GetBookingsBetweenDates(
                fromDate,
                toDate)
            .AsQueryable();


        if (!string.IsNullOrWhiteSpace(route))
        {
            query =
                query.Where(x =>
                x.Route.DepartureStation.Contains(route)
                ||
                x.Route.DestinationStation.Contains(route));
        }

        if (bookingType != null)
        {
            query =
                query.Where(x =>
                x.BookingType == bookingType);
        }

        return query.ToList();
    }


    // Route Frequency
    public Dictionary<string, int>
        GetRouteFrequency(
            DateTime fromDate,
            DateTime toDate)
    {

        return repository
            .GetBookingsBetweenDates(
                fromDate,
                toDate)

            .GroupBy(x =>
                x.Route.DepartureStation
                +
                " → "
                +
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

        return repository
            .GetBookingsBetweenDates(
                fromDate,
                toDate)

            .Sum(x => x.TicketPrice);

    }


    // Selected Booking Cost
    public decimal GetSelectedBookingsCost(
        List<int> bookingIds)
    {

        return repository
            .GetBookingsByIds(bookingIds)

            .Sum(x =>
                x.TicketPrice);

    }

    // Dashboard Summary
    public WeeklyReportSummaryDto
        GetWeeklySummary(
            DateTime startDate)
    {

        DateTime endDate =
            startDate.AddDays(6);

        var bookings =
            repository.GetBookingsBetweenDates(
                startDate,
                endDate);

        return new WeeklyReportSummaryDto
        {

            TotalBookings =
                bookings.Count,

            TotalTicketCost =
                bookings.Sum(
                    x => x.TicketPrice),

            TotalSpecialRequests =
                bookings.Sum(
                    x =>
                    x.SpecialRequests.Count),

            MostPopularRoute =
                bookings
                .GroupBy(x =>
                    x.Route.DepartureStation
                    +
                    " → "
                    +
                    x.Route.DestinationStation)

                .OrderByDescending(
                    x => x.Count())

                .Select(x =>
                    x.Key)

                .FirstOrDefault()
                ??
                "No Bookings"
        };
    }
}