using Microsoft.EntityFrameworkCore;
using TrainTicket.API.Data;
using TrainTicket.API.Models.DTOs;

namespace TrainTicket.API.Services;

public class ChatbotService
{
    private readonly AppDbContext context;

    public ChatbotService(AppDbContext context)
    {
        this.context = context;
    }

    // Main chatbot response
    public ChatbotResponseDto Ask(string question)
    {
        string text = question.ToLower();

        if (text.Contains("availability") || text.Contains("seat"))
        {
            return new ChatbotResponseDto
            {
                Response = "Please provide travel date and route to predict seat availability."
            };
        }

        if (text.Contains("price") || text.Contains("cost"))
        {
            var bookings = context.Bookings.ToList();
            if (bookings.Count == 0)
            {
                return new ChatbotResponseDto
                {
                    Response = "Not enough booking history available for price prediction."
                };
            }
            decimal averagePrice = bookings.Average(x => x.TicketPrice);
            return new ChatbotResponseDto
            {
                Response = $"Based on previous booking patterns, average ticket price is Rs. {averagePrice}. Future prices are expected around this range."
            };
        }

        return new ChatbotResponseDto
        {
            Response = "I can predict seat availability and ticket price trends."
        };
    }

    // Availability prediction engine
    public AvailabilityPredictionDto PredictAvailability(DateTime date, string departure, string destination)
    {
        const int totalSeats = 50;

        var bookings = context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Where(b =>
                b.Route.DepartureStation == departure &&
                b.Route.DestinationStation == destination)
            .ToList();

        int currentBookings = bookings
            .Count(b => b.Schedule.TravelDate.Date == date.Date);

        int historicalBookings = bookings
            .Count(b => b.Schedule.TravelDate.DayOfWeek == date.DayOfWeek);

        bool peakDay = date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Sunday;

        int predictedBookings = historicalBookings;
        if (peakDay) predictedBookings += 10;
        if (currentBookings > predictedBookings) predictedBookings = currentBookings;

        int availableSeats = totalSeats - predictedBookings;
        if (availableSeats < 0) availableSeats = 0;

        string status;
        if (availableSeats > 30)
            status = "High Availability";
        else if (availableSeats > 10)
            status = "Moderate Availability";
        else
            status = "Low Availability";

        string recommendation = status switch
        {
            "High Availability" => "Seats are expected to be available. Normal booking is recommended.",
            "Moderate Availability" => "Demand is increasing. Booking earlier is recommended.",
            _ => "High demand expected. Immediate booking is recommended."
        };

        return new AvailabilityPredictionDto
        {
            TravelDate = date,
            Route = $"{departure} -> {destination}",
            AvailableSeats = availableSeats,
            AvailabilityStatus = status,
            Recommendation = recommendation
        };
    }

    // Price prediction
    public object PredictPrice()
    {
        var bookings = context.Bookings.ToList();
        if (bookings.Count == 0)
        {
            return new { message = "Not enough historical data." };
        }

        decimal averagePrice = bookings.Average(b => b.TicketPrice);
        decimal highestPrice = bookings.Max(b => b.TicketPrice);
        decimal lowestPrice = bookings.Min(b => b.TicketPrice);

        return new
        {
            AveragePrice = averagePrice,
            HighestRecordedPrice = highestPrice,
            LowestRecordedPrice = lowestPrice,
            Prediction = "Future ticket prices are expected to remain within the historical price range."
        };
    }
}