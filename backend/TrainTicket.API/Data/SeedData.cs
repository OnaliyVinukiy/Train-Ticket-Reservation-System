using TrainTicket.API.Models;
using TrainTicket.API.Repositories;

namespace TrainTicket.API.Data;

public static class SeedData
{
    public static void Initialize(
        IRepository<Booking> bookingRepository,
        IRepository<Schedule> scheduleRepository,
        IRepository<SpecialRequest> requestRepository)
    {
        // Avoid seeding twice
        if (bookingRepository.GetAll().Any())
            return;

        var schedule1 = new Schedule
        {
            TravelDate = new DateTime(2026, 8, 10),
            DepartureTime = new TimeSpan(8, 0, 0),
            ArrivalTime = new TimeSpan(10, 30, 0)
        };

        scheduleRepository.Add(schedule1);

        var request1 = new SpecialRequest
        {
            Description = "Window Seat"
        };

        requestRepository.Add(request1);

        var booking1 = new Booking
        {
            BookingReference = "BK001",
            SeatNumber = "A12",
            TicketPrice = 1200,
            Route = new Models.Route
            {
                DepartureStation = "Colombo Fort",
                DestinationStation = "Kandy"
            },
            Schedule = schedule1,
            SpecialRequests = new List<SpecialRequest>
            {
                request1
            }
        };

        bookingRepository.Add(booking1);

    }
}