using TrainTicket.API.DTOs;
using TrainTicket.API.Models;


namespace TrainTicket.API.Helpers;


public static class BookingMapper
{
    public static Booking ToModel(BookingDto dto)
    {

        return new Booking
        {
            SeatNumber = dto.SeatNumber,

            TicketPrice = dto.TicketPrice,

            BookingType = dto.BookingType,


            Route = new Models.Route
            {
                DepartureStation = dto.Route.DepartureStation,

                DestinationStation = dto.Route.DestinationStation
            },


            Schedule = new Schedule
            {
                TravelDate = dto.Schedule.TravelDate,

                DepartureTime = dto.Schedule.DepartureTime,

                ArrivalTime = dto.Schedule.ArrivalTime
            },


            SpecialRequests = dto.SpecialRequests
                .Select(x => new SpecialRequest
                {
                    Description = x.Description
                })
                .ToList()
        };
    }
}