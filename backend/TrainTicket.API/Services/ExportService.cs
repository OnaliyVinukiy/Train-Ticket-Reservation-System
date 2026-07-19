using System.Text;
using TrainTicket.API.Models;

namespace TrainTicket.API.Services;

public class ExportService
{
    public byte[] ExportBookingsToCsv(List<Booking> bookings)
    {
        StringBuilder csv = new();
        csv.AppendLine("Booking Reference,Route,Travel Date,Departure Time,Arrival Time,Seat Number,Ticket Price,Booking Type,Special Requests");

        foreach (var booking in bookings)
        {
            string requests = string.Join(" | ", booking.SpecialRequests.Select(x => x.Description));
            csv.AppendLine($"{booking.BookingReference}," +
                $"{booking.Route.DepartureStation} to {booking.Route.DestinationStation}," +
                $"{booking.Schedule.TravelDate:yyyy-MM-dd}," +
                $"{booking.Schedule.DepartureTime}," +
                $"{booking.Schedule.ArrivalTime}," +
                $"{booking.SeatNumber}," +
                $"{booking.TicketPrice}," +
                $"{booking.BookingType}," +
                $"{requests}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}