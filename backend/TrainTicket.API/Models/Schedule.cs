namespace TrainTicket.API.Models;

public class Schedule
{
    public int Id { get; set; }

    public DateTime TravelDate { get; set; }

    public TimeSpan DepartureTime { get; set; }

    public TimeSpan ArrivalTime { get; set; }
}