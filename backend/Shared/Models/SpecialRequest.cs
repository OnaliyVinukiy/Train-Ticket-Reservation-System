using System.Text.Json.Serialization;

namespace TrainTicket.API.Models;

public class SpecialRequest
{
    public int Id { get; set; }

    public string Description { get; set; } = "";


    public int BookingId { get; set; }


    [JsonIgnore]
    public Booking? Booking { get; set; }
}