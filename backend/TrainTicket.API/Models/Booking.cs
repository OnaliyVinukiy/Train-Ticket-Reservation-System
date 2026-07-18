namespace TrainTicket.API.Models;

public class Booking
{
    public int Id { get; set; }

    public string BookingReference { get; set; } = "";

    public string SeatNumber { get; set; } = "";

    public decimal TicketPrice { get; set; }

    public BookingType BookingType { get; set; }


    public int RouteId { get; set; }

    public Route Route { get; set; } = null!;


    public int ScheduleId { get; set; }

    public Schedule Schedule { get; set; } = null!;


    public List<SpecialRequest> SpecialRequests { get; set; } = new();
}