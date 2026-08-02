using System.Text.Json.Serialization;

namespace Shared.Models;

public class Schedule
{
    public int Id { get; set; }

    public DateTime TravelDate { get; set; }

    public TimeSpan DepartureTime { get; set; }

    public TimeSpan ArrivalTime { get; set; }


    [JsonIgnore]
    public List<Booking> Bookings { get; set; } = new();
}