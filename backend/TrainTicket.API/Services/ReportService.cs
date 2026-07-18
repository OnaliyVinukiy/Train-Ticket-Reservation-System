using TrainTicket.API.Models.DTOs;
using TrainTicket.API.Repositories;

namespace TrainTicket.API.Services;

public class ReportService
{
    private readonly BookingRepository repository;

    public ReportService(BookingRepository repository)
    {
        this.repository = repository;
    }

    public List<WeeklyReportDto> GetWeeklyReport(DateTime startDate)
    {
        var bookings = repository.GetAllBookings();
        var report = new List<WeeklyReportDto>();

        for (int i = 0; i < 7; i++)
        {
            var currentDate = startDate.AddDays(i);
            var dailyBookings = bookings
                .Where(b => b.Schedule.TravelDate.Date == currentDate.Date)
                .Select(b => new WeeklyBookingDto
                {
                    BookingId = b.Id,
                    Route = $"{b.Route.DepartureStation} → {b.Route.DestinationStation}",
                    SeatNumber = b.SeatNumber,
                    TicketPrice = b.TicketPrice,
                    SpecialRequests = b.SpecialRequests.Select(r => r.Description).ToList()
                })
                .ToList();

            report.Add(new WeeklyReportDto
            {
                Date = currentDate,
                Day = currentDate.DayOfWeek.ToString(),
                Bookings = dailyBookings
            });
        }

        return report;
    }
}