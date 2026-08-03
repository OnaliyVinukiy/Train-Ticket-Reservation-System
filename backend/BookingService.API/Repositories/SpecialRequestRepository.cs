using BookingService.API.Data;
using BookingService.API.Models;

namespace BookingService.API.Repositories;

public class SpecialRequestRepository
{
    private readonly AppDbContext context;

    public SpecialRequestRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<SpecialRequest> GetRequests()
    {
        return context.SpecialRequests.ToList();
    }

    public SpecialRequest? GetRequest(int id)
    {
        return context.SpecialRequests.Find(id);
    }

    public SpecialRequest CreateRequest(SpecialRequest request)
    {
        context.SpecialRequests.Add(request);
        context.SaveChanges();

        return request;
    }

    public void UpdateRequest(SpecialRequest request)
    {
        context.SpecialRequests.Update(request);
        context.SaveChanges();
    }

    public void DeleteRequest(int id)
    {
        var request = context.SpecialRequests.Find(id);

        if (request != null)
        {
            context.SpecialRequests.Remove(request);
            context.SaveChanges();
        }
    }
}