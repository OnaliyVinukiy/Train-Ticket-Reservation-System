using TrainTicket.API.DTOs;
using TrainTicket.API.Repositories;
using TrainTicket.API.Models;


namespace TrainTicket.API.Services;


public class ReportService
{

    private readonly IRepository<Booking> repository;

    public ReportService(
        IRepository<Booking> repository)
    {
        this.repository = repository;
    }


    public IEnumerable<WeeklyReportDto> GenerateWeeklyReport()
    {

        var bookings = repository.GetAll();

        return bookings
            .GroupBy(b =>
                b.Schedule.TravelDate.DayOfWeek)
            .Select(group => new WeeklyReportDto
            {

                Day = group.Key.ToString(),

                BookingCount = group.Count(),

                SpecialRequests =
                    group
                    .SelectMany(
                        b => b.SpecialRequests
                    )
                    .Select(
                        r => r.Description
                    )
                    .ToList()

            });
    }

}