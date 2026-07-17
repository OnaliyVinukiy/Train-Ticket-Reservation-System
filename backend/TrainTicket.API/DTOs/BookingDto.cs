namespace TrainTicket.API.DTOs;

public class BookingDto
{
    public string SeatNumber { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }


    public RouteDto Route { get; set; } = new();

    public ScheduleDto Schedule { get; set; } = new();


    public List<SpecialRequestDto> SpecialRequests { get; set; } = new();
}