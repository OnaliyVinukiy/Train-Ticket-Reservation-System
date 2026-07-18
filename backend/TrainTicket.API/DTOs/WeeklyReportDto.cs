namespace TrainTicket.API.Models.DTOs;

public class WeeklyReportDto
{

    public DateTime Date { get; set; }

    public string Day { get; set; } = "";

    public List<WeeklyBookingDto> Bookings { get; set; } = new();

}


public class WeeklyBookingDto
{

    public int BookingId { get; set; }

    public string Route { get; set; } = "";

    public string SeatNumber { get; set; } = "";

    public decimal TicketPrice { get; set; }

    public List<string> SpecialRequests { get; set; } = new();
}