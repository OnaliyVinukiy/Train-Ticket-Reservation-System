using System.Text.Json.Serialization;

namespace TrainTicket.API.Models;

public class Route
{
    public int Id { get; set; }

    public string DepartureStation { get; set; } = "";

    public string DestinationStation { get; set; } = "";


    [JsonIgnore]
    public List<Booking> Bookings { get; set; } = new();
}