namespace TrainTicket.API.Models;

public class RecurringBooking : Booking
{
    public string RecurrencePattern { get; set; } = string.Empty;

    public DateTime RecurrenceEndDate { get; set; }
}