using TrainTicket.API.Models;
using TrainTicket.API.Repositories;

namespace TrainTicket.API.Services;

public class BookingService
{
    private readonly IRepository<Booking> repository;


    public BookingService(IRepository<Booking> repository)
    {
        this.repository = repository;
    }


    public IEnumerable<Booking> GetAllBookings()
    {
        return repository.GetAll();
    }


    public Booking? GetBookingById(int id)
    {
        return repository.GetById(id);
    }


    public Booking CreateBooking(Booking booking)
    {
        repository.Add(booking);

        return booking;
    }


    public void UpdateBooking(Booking booking)
    {
        repository.Update(booking);
    }


    public void DeleteBooking(int id)
    {
        repository.Delete(id);
    }
}