using System.ComponentModel.DataAnnotations;
using BookingService.API.Models;

namespace BookingService.API.DTOs;

public class BookingDto
{
    public int Id { get; set; }

    public string BookingReference { get; set; } = "";

    public string SeatNumber { get; set; } = "";

    public decimal TicketPrice { get; set; }

    public BookingType BookingType { get; set; }

    public RecurrencePattern RecurrencePattern { get; set; }

    public DateTime? RecurrenceEndDate { get; set; }

    public RouteDto Route { get; set; } = new();

    public ScheduleDto Schedule { get; set; } = new();

    public List<SpecialRequestDto> SpecialRequests { get; set; }
        = new();
}