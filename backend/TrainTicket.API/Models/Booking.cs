using System.ComponentModel.DataAnnotations;

namespace TrainTicket.API.Models;

public class Booking
{
    public int Id { get; set; }

    public string BookingReference { get; set; } = Guid.NewGuid().ToString();


    [Required]
    public string SeatNumber { get; set; } = string.Empty;


    [Range(0.01, double.MaxValue)]
    public decimal TicketPrice { get; set; }


    public BookingType BookingType { get; set; } = BookingType.OneOff;


    public Route Route { get; set; } = new();

    public Schedule Schedule { get; set; } = new();


    public List<SpecialRequest> SpecialRequests { get; set; } = new();
}