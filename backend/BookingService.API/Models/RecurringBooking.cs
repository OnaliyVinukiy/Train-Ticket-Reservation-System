namespace TrainTicket.API.Models;

public class RecurringBooking : Booking
{
    public RecurrencePattern RecurrencePattern { get; set; }


    public DateTime RecurrenceEndDate { get; set; }
    public RecurringBooking()
    {
        BookingType = BookingType.Recurring;
    }
}