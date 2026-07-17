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
        // Prevent duplicate data
        if (bookingRepository.GetAll().Any())
        {
            return;
        }


        // Schedule 1
        var schedule1 = new Schedule
        {
            TravelDate = new DateTime(2026, 8, 3),
            DepartureTime = new TimeSpan(8, 0, 0),
            ArrivalTime = new TimeSpan(10, 30, 0)
        };


        // Schedule 2
        var schedule2 = new Schedule
        {
            TravelDate = new DateTime(2026, 8, 5),
            DepartureTime = new TimeSpan(9, 15, 0),
            ArrivalTime = new TimeSpan(11, 45, 0)
        };


        // Schedule 3
        var schedule3 = new Schedule
        {
            TravelDate = new DateTime(2026, 8, 7),
            DepartureTime = new TimeSpan(14, 0, 0),
            ArrivalTime = new TimeSpan(16, 30, 0)
        };


        scheduleRepository.Add(schedule1);
        scheduleRepository.Add(schedule2);
        scheduleRepository.Add(schedule3);



        // Special Requests

        var request1 = new SpecialRequest
        {
            Description = "Window seat"
        };


        var request2 = new SpecialRequest
        {
            Description = "Wheelchair assistance"
        };


        var request3 = new SpecialRequest
        {
            Description = "Extra luggage"
        };


        requestRepository.Add(request1);
        requestRepository.Add(request2);
        requestRepository.Add(request3);



        // Bookings

        var booking1 = new Booking
        {
            BookingReference = "BK001",

            SeatNumber = "A12",

            TicketPrice = 1200,

            BookingType = BookingType.OneOff,


            Route = new Models.Route
            {
                DepartureStation = "Colombo Fort",

                DestinationStation = "Kandy"
            },


            Schedule = schedule1,


            SpecialRequests =
            [
                request1
            ]
        };



        var booking2 = new Booking
        {
            BookingReference = "BK002",

            SeatNumber = "B05",

            TicketPrice = 850,

            BookingType = BookingType.OneOff,


            Route = new Models.Route
            {
                DepartureStation = "Colombo Fort",

                DestinationStation = "Galle"
            },


            Schedule = schedule2,


            SpecialRequests =
            [
                request2
            ]
        };



        var booking3 = new Booking
        {
            BookingReference = "BK003",

            SeatNumber = "C20",

            TicketPrice = 950,

            BookingType = BookingType.OneOff,


            Route = new Models.Route
            {
                DepartureStation = "Kandy",

                DestinationStation = "Colombo Fort"
            },


            Schedule = schedule3,


            SpecialRequests =
            [
                request3
            ]
        };


        bookingRepository.Add(booking1);

        bookingRepository.Add(booking2);

        bookingRepository.Add(booking3);
    }
}