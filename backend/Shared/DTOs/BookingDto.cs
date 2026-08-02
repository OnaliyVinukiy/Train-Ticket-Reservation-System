using System.ComponentModel.DataAnnotations;
using TrainTicket.API.Models;


namespace TrainTicket.API.DTOs;


public class BookingDto
{
    [Required]
    public string SeatNumber { get; set; } = string.Empty;


    [Range(0.01, double.MaxValue)]
    public decimal TicketPrice { get; set; }


    public BookingType BookingType { get; set; } = BookingType.OneOff;


    public RouteDto Route { get; set; } = new();


    public ScheduleDto Schedule { get; set; } = new();


    public List<SpecialRequestDto> SpecialRequests { get; set; } = new();
}