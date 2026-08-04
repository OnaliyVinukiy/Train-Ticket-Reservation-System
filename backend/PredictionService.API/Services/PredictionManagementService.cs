using Microsoft.EntityFrameworkCore;
using PredictionService.API.Data;
using PredictionService.API.DTOs;
using PredictionService.API.Models;

namespace PredictionService.API.Services;

public class PredictionManagementService
{
    private readonly AppDbContext context;

    public PredictionManagementService(AppDbContext context)
    {
        this.context = context;
    }


    public async Task<PredictionResponseDto> Predict(
        string route,
        DateTime travelDate,
        string departureTime)
    {
        var routeParts = route.Split("→");

        if (routeParts.Length != 2)
        {
            return new PredictionResponseDto
            {
                Route = route,
                TravelDate = travelDate,
                DepartureTime = departureTime,
                AvailabilityStatus = "Unknown",
                Recommendation = "Invalid route format."
            };
        }

        string departureStation =
            routeParts[0].Trim();

        string destinationStation =
            routeParts[1].Trim();

        // Historical bookings
        var historicalBookings =
            await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Where(b =>
                b.Route.DepartureStation == departureStation &&
                b.Route.DestinationStation == destinationStation &&
                b.Schedule.TravelDate < travelDate)
            .ToListAsync();

        int totalHistoricalBookings =
            historicalBookings.Count;

        double demandScore = 0;

        List<string> factors = new();


        demandScore += totalHistoricalBookings * 5;

        factors.Add(
            $"Historical demand: {totalHistoricalBookings} previous bookings");


        var upcomingBookings =
            await context.Bookings
            .Include(b => b.Route)
            .Include(b => b.Schedule)
            .Where(b =>
                b.Route.DepartureStation == departureStation &&
                b.Route.DestinationStation == destinationStation &&
                b.Schedule.TravelDate.Date == travelDate.Date)
            .ToListAsync();


        demandScore += upcomingBookings.Count * 8;

        if (upcomingBookings.Count > 0)
        {
            factors.Add(
                $"{upcomingBookings.Count} existing bookings already made");
        }


        if (TimeSpan.TryParse(
            departureTime,
            out TimeSpan time))
        {
            if (
                (time >= new TimeSpan(6, 0, 0) &&
                 time <= new TimeSpan(9, 0, 0))
                ||
                (time >= new TimeSpan(16, 0, 0) &&
                 time <= new TimeSpan(19, 0, 0))
            )
            {
                demandScore += 20;

                factors.Add(
                    "Peak travel time increases demand");
            }
        }


        if (
            travelDate.DayOfWeek == DayOfWeek.Saturday ||
            travelDate.DayOfWeek == DayOfWeek.Sunday)
        {
            demandScore += 15;

            factors.Add(
                "Weekend travel increases demand");
        }


        var recurringBookings =
            await context.Bookings
            .Where(b =>
                b.BookingType == BookingType.Recurring &&
                b.Route.DepartureStation == departureStation &&
                b.Route.DestinationStation == destinationStation)
            .CountAsync();


        if (recurringBookings > 0)
        {
            demandScore += recurringBookings * 3;

            factors.Add(
                $"{recurringBookings} recurring passengers detected");
        }


        string availability =
            demandScore switch
            {
                >= 120 => "Very Low availability",
                >= 80 => "Low availability",
                >= 40 => "Medium availability",
                _ => "High availability"
            };


        decimal averagePrice =
            historicalBookings.Any()
            ? historicalBookings.Average(x => x.TicketPrice)
            : 0;


        string priceTrend =
            demandScore >= 80
            ? "Expected increase in ticket price due to high demand"
            : demandScore >= 40
            ? "Possible slight price increase"
            : "Expected stable pricing";


        string recommendation =
            demandScore >= 80
            ? "High demand expected. Booking early is recommended."
            : "Seats are likely available. Normal booking time is acceptable.";


        return new PredictionResponseDto
        {
            Route = route,
            TravelDate = travelDate,
            DepartureTime = departureTime,
            HistoricalBookings = totalHistoricalBookings,
            DemandScore = Math.Round(demandScore, 2),
            AvailabilityStatus = availability,
            PriceTrend = priceTrend,
            AverageHistoricalPrice = averagePrice,
            Factors = factors,
            Recommendation = recommendation
        };
    }
}