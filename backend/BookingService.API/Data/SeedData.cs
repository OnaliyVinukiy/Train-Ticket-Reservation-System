using TrainTicket.API.Models;
using Route = TrainTicket.API.Models.Route;

namespace TrainTicket.API.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Bookings.Any())
            return;

        // ==========================
        // Routes
        // ==========================

        var colomboKandy = new Route
        {
            DepartureStation = "Colombo Fort",
            DestinationStation = "Kandy"
        };

        var colomboGalle = new Route
        {
            DepartureStation = "Colombo Fort",
            DestinationStation = "Galle"
        };

        var colomboJaffna = new Route
        {
            DepartureStation = "Colombo Fort",
            DestinationStation = "Jaffna"
        };

        var kandyBadulla = new Route
        {
            DepartureStation = "Kandy",
            DestinationStation = "Badulla"
        };

        context.Routes.AddRange(
            colomboKandy,
            colomboGalle,
            colomboJaffna,
            kandyBadulla
        );

        context.SaveChanges();

        // ==========================
        // Bookings
        // ==========================

        AddBooking(
            context,
            "TRN001",
            colomboKandy,
            new DateTime(2026, 7, 20),
            new TimeSpan(6, 30, 0),
            new TimeSpan(9, 0, 0),
            "A12",
            1800,
            BookingType.OneOff,
            "Window Seat"
        );

        AddBooking(
            context,
            "TRN002",
            colomboKandy,
            new DateTime(2026, 7, 22),
            new TimeSpan(6, 30, 0),
            new TimeSpan(9, 0, 0),
            "A18",
            1850,
            BookingType.Recurring,
            "Extra Luggage"
        );

        AddBooking(
            context,
            "TRN003",
            colomboGalle,
            new DateTime(2026, 7, 23),
            new TimeSpan(8, 0, 0),
            new TimeSpan(10, 15, 0),
            "B05",
            1450,
            BookingType.OneOff,
            "Near Exit"
        );

        AddBooking(
            context,
            "TRN004",
            colomboGalle,
            new DateTime(2026, 7, 24),
            new TimeSpan(8, 0, 0),
            new TimeSpan(10, 15, 0),
            "B10",
            1500,
            BookingType.OneOff,
            "Window Seat"
        );

        AddBooking(
            context,
            "TRN005",
            colomboJaffna,
            new DateTime(2026, 7, 25),
            new TimeSpan(20, 0, 0),
            new TimeSpan(5, 45, 0),
            "C08",
            3900,
            BookingType.OneOff,
            "Sleeper Preference"
        );

        AddBooking(
            context,
            "TRN006",
            kandyBadulla,
            new DateTime(2026, 7, 26),
            new TimeSpan(9, 30, 0),
            new TimeSpan(13, 0, 0),
            "D11",
            1200,
            BookingType.Recurring,
            "Window Seat"
        );

        AddBooking(
            context,
            "TRN007",
            colomboKandy,
            new DateTime(2026, 8, 1),
            new TimeSpan(6, 30, 0),
            new TimeSpan(9, 0, 0),
            "A07",
            1900,
            BookingType.OneOff,
            "Wheelchair Assistance"
        );

        AddBooking(
            context,
            "TRN008",
            colomboKandy,
            new DateTime(2026, 8, 2),
            new TimeSpan(6, 30, 0),
            new TimeSpan(9, 0, 0),
            "A20",
            1950,
            BookingType.OneOff,
            "Extra Luggage"
        );

        context.SaveChanges();
    }

    private static void AddBooking(
        AppDbContext context,
        string reference,
        Route route,
        DateTime date,
        TimeSpan departure,
        TimeSpan arrival,
        string seat,
        decimal price,
        BookingType type,
        string request)
    {
        var booking = new Booking
        {
            BookingReference = reference,
            Route = route,
            BookingType = type,
            SeatNumber = seat,
            TicketPrice = price,
            Schedule = new Schedule
            {
                TravelDate = date,
                DepartureTime = departure,
                ArrivalTime = arrival
            },
            SpecialRequests =
            [
                new SpecialRequest
                {
                    Description = request
                }
            ]
        };

        context.Bookings.Add(booking);
    }
}