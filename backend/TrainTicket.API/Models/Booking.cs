namespace TrainTicket.API.Models;

public class Booking
{
    public int Id { get; set; }

    public string BookingReference { get; set; } = Guid.NewGuid().ToString();

    public string SeatNumber { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }


    public Route Route { get; set; } = new();

    public Schedule Schedule { get; set; } = new();


    public List<SpecialRequest> SpecialRequests { get; set; } = new();
}