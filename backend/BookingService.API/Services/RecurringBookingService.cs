using BookingService.API.Models;
using BookingService.API.Repositories;

namespace BookingService.API.Services;

public class RecurringBookingService
{
    private readonly BookingRepository repository;


    public RecurringBookingService(
        BookingRepository repository)
    {
        this.repository = repository;
    }


    public async Task<List<Booking>> GenerateRecurringBookings(
        RecurringBooking recurringBooking)
    {
        var generatedBookings = new List<Booking>();

        DateTime currentDate =
            recurringBooking.Schedule.TravelDate;


        while (currentDate <= recurringBooking.RecurrenceEndDate)
        {
            bool exists =
                await repository.BookingExists(
                    currentDate,
                    recurringBooking.Route.DepartureStation,
                    recurringBooking.Route.DestinationStation
                );


            if (!exists)
            {
                var booking = CreateBooking(
                    recurringBooking,
                    currentDate
                );

                await repository.CreateBooking(booking);
                generatedBookings.Add(booking);
            }


            currentDate =
                GetNextDate(
                    currentDate,
                    recurringBooking.RecurrencePattern);
        }

        return generatedBookings;
    }


    private Booking CreateBooking(
        RecurringBooking recurringBooking,
        DateTime date)
    {
        return new Booking
        {
            BookingReference = Guid.NewGuid()
                .ToString()
                .Substring(0, 8)
                .ToUpper(),


            SeatNumber = recurringBooking.SeatNumber,

            TicketPrice = recurringBooking.TicketPrice,

            BookingType = BookingType.Recurring,


            RecurrencePattern =
                recurringBooking.RecurrencePattern,


            RecurrenceEndDate =
                recurringBooking.RecurrenceEndDate,


            Route = new Models.Route
            {
                DepartureStation =
                    recurringBooking.Route.DepartureStation,


                DestinationStation =
                    recurringBooking.Route.DestinationStation
            },


            Schedule = new Schedule
            {
                TravelDate = date,


                DepartureTime =
                    recurringBooking.Schedule.DepartureTime,


                ArrivalTime =
                    recurringBooking.Schedule.ArrivalTime
            },


            SpecialRequests =
                recurringBooking.SpecialRequests
                .Select(x => new SpecialRequest
                {
                    Description = x.Description
                })
                .ToList()
        };
    }


    private DateTime GetNextDate(
        DateTime date,
        RecurrencePattern pattern)
    {
        return pattern switch
        {
            RecurrencePattern.Daily =>
                date.AddDays(1),

            RecurrencePattern.Weekly =>
                date.AddDays(7),

            RecurrencePattern.Monthly =>
                date.AddMonths(1),

            _ =>
                date.AddDays(1)
        };
    }
}