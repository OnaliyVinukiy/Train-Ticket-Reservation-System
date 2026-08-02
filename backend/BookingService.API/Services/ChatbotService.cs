using System.Text.RegularExpressions;
using TrainTicket.API.Models;
using TrainTicket.API.Models.DTOs;

namespace TrainTicket.API.Services;

public class ChatbotService
{
    private readonly PredictionService predictionService;
    private readonly ReportService reportService;

    public ChatbotService(PredictionService predictionService, ReportService reportService)
    {
        this.predictionService = predictionService;
        this.reportService = reportService;
    }

    public object ProcessMessage(string message)
    {
        message = message.Trim();
        ChatIntent intent = DetectIntent(message);

        switch (intent)
        {
            case ChatIntent.Greeting:
                return new
                {
                    reply =
                        "Hello! I can predict train availability and ticket price trends.\n\n" +
                        "Try asking:\n" +
                        "• Will Colombo to Kandy be available tomorrow?\n" +
                        "• Predict ticket prices for Galle to Colombo\n" +
                        "• What is the most popular route?\n" +
                        "• Show this week's summary"
                };

            case ChatIntent.Help:
                return new
                {
                    reply =
                        "You can ask me things like:\n\n" +
                        "• Will Colombo to Kandy be available tomorrow?\n" +
                        "• Predict prices for Colombo to Badulla\n" +
                        "• Which route is busiest?\n" +
                        "• Show weekly summary"
                };

            case ChatIntent.PopularRoute:
                var summary = reportService.GetWeeklySummary(DateTime.Today);
                return new
                {
                    reply = $"The most popular route this week is {summary.MostPopularRoute}."
                };

            case ChatIntent.WeeklySummary:
                var weekly = reportService.GetWeeklySummary(DateTime.Today);
                return new
                {
                    reply =
$"""
Weekly Summary

Bookings : {weekly.TotalBookings}

Total Cost : Rs. {weekly.TotalTicketCost}

Special Requests : {weekly.TotalSpecialRequests}

Most Popular Route : {weekly.MostPopularRoute}
"""
                };

            case ChatIntent.Availability:
            case ChatIntent.PriceTrend:
            case ChatIntent.Recommendation:
                return HandlePrediction(message);

            default:
                return new
                {
                    reply =
                        "Sorry, I couldn't understand your question. Type 'help' to see what I can do."
                };
        }
    }

    private object HandlePrediction(string message)
    {
        string route = ExtractRoute(message);
        DateTime date = ExtractDate(message);

        var prediction = predictionService.Predict(route, date, "Any");

        return new
        {
            reply =
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
            availability = prediction.AvailabilityStatus,
            priceTrend = prediction.PriceTrend,
            recommendation = prediction.Recommendation,
            factors = prediction.Factors
        };
    }

    private ChatIntent DetectIntent(string message)
    {
        message = message.ToLower();

        if (Regex.IsMatch(message, @"\b(hi|hello|hey)\b"))
            return ChatIntent.Greeting;

        if (message.Contains("help"))
            return ChatIntent.Help;

        if (message.Contains("popular route") || message.Contains("busiest"))
            return ChatIntent.PopularRoute;

        if (message.Contains("summary"))
            return ChatIntent.WeeklySummary;

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
        var routes = new[]
        {
            "Colombo",
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

        foreach (var station in routes)
        {
            if (message.Contains(station, StringComparison.OrdinalIgnoreCase))
            {
                if (departure == null)
                    departure = station;
                else
                    destination = station;
            }
        }

        if (departure != null && destination != null)
            return $"{departure} → {destination}";

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

public enum ChatIntent
{
    Unknown,
    Greeting,
    Help,
    Availability,
    PriceTrend,
    Recommendation,
    PopularRoute,
    WeeklySummary
}