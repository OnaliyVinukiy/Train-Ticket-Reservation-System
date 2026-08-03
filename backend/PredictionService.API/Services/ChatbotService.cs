using System.Text.RegularExpressions;
using PredictionService.API.Models;
using PredictionService.API.DTOs;


namespace PredictionService.API.Services;

public class ChatbotService
{
    private readonly PredictionManagementService predictionService;

    public ChatbotService(PredictionManagementService predictionService)
    {
        this.predictionService = predictionService;
    }

    public ChatbotResponseDto ProcessMessage(string message)
    {
        message = message.Trim();
        ChatIntent intent = DetectIntent(message);

        switch (intent)
        {
            case ChatIntent.Greeting:
                return new ChatbotResponseDto
                {
                    Reply =
                    """
                    Hello! I can help predict train availability and ticket prices.

                    Try asking:
                    • Will Colombo to Kandy be available tomorrow?
                    • Predict prices for Galle to Colombo
                    """
                };

            case ChatIntent.Help:
                return new ChatbotResponseDto
                {
                    Reply =
                        "You can ask me things like:\n\n" +
                        "• Will Colombo to Kandy be available tomorrow?\n" +
                        "• Predict prices for Colombo to Galle\n" +
                        "• Will Colombo to Kandy have seats tomorrow?\n" +
                        "• Predict prices for Colombo to Galle"
                };

            case ChatIntent.Availability:
            case ChatIntent.PriceTrend:
            case ChatIntent.Recommendation:
                return HandlePrediction(message);

            default:
                return new ChatbotResponseDto
                {
                    Reply =
                        "Sorry, I couldn't understand your question. Type 'help' to see what I can do."
                };
        }
    }

    private ChatbotResponseDto HandlePrediction(string message)
    {
        string route = ExtractRoute(message);
        DateTime date = ExtractDate(message);

        var prediction = predictionService.Predict(route, date, "Any");

        return new ChatbotResponseDto
        {
            Reply =
 $"""
Prediction Result

Route:
{prediction.Route}

Travel Date:
{prediction.TravelDate:dd MMM yyyy}

Availability:
{prediction.AvailabilityStatus}

Average Historical Price:
Rs. {prediction.AverageHistoricalPrice}

Price Trend:
{prediction.PriceTrend}

Reason:
{string.Join("\n", prediction.Factors)}

Recommendation:
{prediction.Recommendation}
""",

            Availability = prediction.AvailabilityStatus,

            PriceTrend = prediction.PriceTrend,

            Recommendation = prediction.Recommendation,

            Factors = prediction.Factors
        };
    }

    private ChatIntent DetectIntent(string message)
    {
        message = message.ToLower();

        if (Regex.IsMatch(message, @"\b(hi|hello|hey)\b"))
            return ChatIntent.Greeting;

        if (message.Contains("help"))
            return ChatIntent.Help;

        if (message.Contains("price"))
            return ChatIntent.PriceTrend;

        if (message.Contains("recommend"))
            return ChatIntent.Recommendation;

        if (message.Contains("available") || message.Contains("availability") || message.Contains("seat"))
            return ChatIntent.Availability;

        return ChatIntent.Unknown;
    }

    private string ExtractRoute(string message)
    {
        var stations = new[]
        {
        "Colombo Fort",
        "Kandy",
        "Galle",
        "Jaffna",
        "Badulla",
        "Matara",
        "Anuradhapura",
        "Kurunegala",
        "Negombo"
    };

        string? departure = null;
        string? destination = null;

        foreach (var station in stations)
        {
            if (message.Contains(station, StringComparison.OrdinalIgnoreCase))
            {
                if (departure == null)
                    departure = station;
                else
                {
                    destination = station;
                    break;
                }
            }
        }

        if (departure != null && destination != null)
        {

            return $"{departure} → {destination}";
        }

        return "Unknown Route";
    }
    private DateTime ExtractDate(string message)
    {
        message = message.ToLower();

        if (message.Contains("today"))
            return DateTime.Today;

        if (message.Contains("tomorrow"))
            return DateTime.Today.AddDays(1);

        if (message.Contains("next week"))
            return DateTime.Today.AddDays(7);

        return DateTime.Today;
    }
}