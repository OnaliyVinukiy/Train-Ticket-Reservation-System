namespace TrainTicket.API.Models.DTOs;


public class AvailabilityPredictionDto
{

    public DateTime TravelDate { get; set; }

    public string Route { get; set; } = "";

    public int AvailableSeats { get; set; }

    public string AvailabilityStatus { get; set; } = "";

    public string Recommendation { get; set; } = "";

}