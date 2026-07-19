namespace TrainTicket.API.Models.DTOs;

public class PredictionResponseDto
{
    public string Route { get; set; } = "";

    public DateTime TravelDate { get; set; }

    public string DepartureTime { get; set; } = "";

    public int HistoricalBookings { get; set; }

    public double DemandScore { get; set; }

    public string AvailabilityStatus { get; set; } = "";

    public string PriceTrend { get; set; } = "";

    public decimal AverageHistoricalPrice { get; set; }

    public List<string> Factors { get; set; } = new();

    public string Recommendation { get; set; } = "";
}