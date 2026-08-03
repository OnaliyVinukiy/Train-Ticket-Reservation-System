using BookingService.API.DTOs;
using BookingService.API.Models;
using Route = BookingService.API.Models.Route;

namespace BookingService.API.Helpers;

public static class BookingMapper
{
    public static Booking ToModel(BookingDto dto)
    {
        return new Booking
        {
            SeatNumber = dto.SeatNumber,

            TicketPrice = dto.TicketPrice,

            BookingType = dto.BookingType,


            Route = new Route
            {
                DepartureStation =
                    dto.Route.DepartureStation,

                DestinationStation =
                    dto.Route.DestinationStation
            },


            Schedule = new Schedule
            {
                TravelDate =
                    dto.Schedule.TravelDate,

                DepartureTime =
                    dto.Schedule.DepartureTime,

                ArrivalTime =
                    dto.Schedule.ArrivalTime
            },


            SpecialRequests =
                dto.SpecialRequests?
                .Select(x => new SpecialRequest
                {
                    Description = x.Description
                })
                .ToList()
                ??
                new List<SpecialRequest>()
        };
    }



    public static BookingDto ToDto(Booking booking)
    {
        return new BookingDto
        {
            SeatNumber = booking.SeatNumber,

            TicketPrice = booking.TicketPrice,

            BookingType = booking.BookingType,


            Route = new RouteDto
            {
                DepartureStation =
                    booking.Route.DepartureStation,

                DestinationStation =
                    booking.Route.DestinationStation
            },


            Schedule = new ScheduleDto
            {
                Id = booking.Schedule.Id,

                TravelDate =
                    booking.Schedule.TravelDate,

                DepartureTime =
                    booking.Schedule.DepartureTime,

                ArrivalTime =
                    booking.Schedule.ArrivalTime
            },


            SpecialRequests =
                booking.SpecialRequests
                .Select(x => new SpecialRequestDto
                {
                    Description = x.Description
                })
                .ToList()
        };
    }
}