using TrainTicket.API.Models;
using TrainTicket.API.Repositories;

namespace TrainTicket.API.Services;

public class RecurringBookingService
{
    private readonly BookingRepository repository;

    public RecurringBookingService(BookingRepository repository)
    {
        this.repository = repository;
    }

    public List<Booking> GenerateRecurringBookings(RecurringBooking recurringBooking)
    {
        var generatedBookings = new List<Booking>();
        DateTime currentDate = recurringBooking.Schedule.TravelDate;

        while (currentDate <= recurringBooking.RecurrenceEndDate)
        {
            var booking = new Booking
            {
                BookingReference = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                SeatNumber = recurringBooking.SeatNumber,
                TicketPrice = recurringBooking.TicketPrice,
                BookingType = BookingType.OneOff,
                Route = new Models.Route
                {
                    DepartureStation = recurringBooking.Route.DepartureStation,
                    DestinationStation = recurringBooking.Route.DestinationStation
                },
                Schedule = new Schedule
                {
                    TravelDate = currentDate,
                    DepartureTime = recurringBooking.Schedule.DepartureTime,
                    ArrivalTime = recurringBooking.Schedule.ArrivalTime
                },
                SpecialRequests = recurringBooking.SpecialRequests.Select(x => new SpecialRequest
                {
                    Description = x.Description
                }).ToList()
            };

            repository.CreateBooking(booking);
            generatedBookings.Add(booking);

            currentDate = GetNextDate(currentDate, recurringBooking.RecurrencePattern);
        }

        return generatedBookings;
    }

    private DateTime GetNextDate(DateTime date, RecurrencePattern pattern)
    {
        return pattern switch
        {
            RecurrencePattern.Daily => date.AddDays(1),
            RecurrencePattern.Weekly => date.AddDays(7),
            RecurrencePattern.Monthly => date.AddMonths(1),
            _ => date.AddDays(1)
        };
    }
}