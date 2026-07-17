namespace TrainTicket.API.Models;

public class Route
{
    public int Id { get; set; }

    public string DepartureStation { get; set; } = string.Empty;

    public string DestinationStation { get; set; } = string.Empty;
}