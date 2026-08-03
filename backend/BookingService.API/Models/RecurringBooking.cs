namespace BookingService.API.Models;

public class RecurringBooking : Booking
{
    public RecurringBooking()
    {
        BookingType = BookingType.Recurring;
    }
}