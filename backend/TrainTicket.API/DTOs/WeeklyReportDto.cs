namespace TrainTicket.API.DTOs;

public class WeeklyReportDto
{
    public string Day { get; set; } = string.Empty;

    public int BookingCount { get; set; }

    public List<string> SpecialRequests { get; set; } = new();
}