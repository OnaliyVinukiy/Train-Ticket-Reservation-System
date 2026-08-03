namespace PredictionService.API.DTOs;

public class ChatbotResponseDto
{
    public string Reply { get; set; } = "";

    public string Availability { get; set; } = "";

    public string PriceTrend { get; set; } = "";

    public string Recommendation { get; set; } = "";
}