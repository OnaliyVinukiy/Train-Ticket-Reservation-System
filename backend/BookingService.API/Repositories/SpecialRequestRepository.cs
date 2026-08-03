using BookingService.API.Data;
using BookingService.API.Models;
using Microsoft.EntityFrameworkCore;

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
        return context.SpecialRequests
            .AsNoTracking()
            .ToList();
    }


    public SpecialRequest? GetRequest(int id)
    {
        return context.SpecialRequests
            .FirstOrDefault(x => x.Id == id);
    }


    public SpecialRequest CreateRequest(SpecialRequest request)
    {
        context.SpecialRequests.Add(request);
        context.SaveChanges();

        return request;
    }


    public void UpdateRequest(SpecialRequest request)
    {
        var existingRequest =
            context.SpecialRequests
            .FirstOrDefault(x => x.Id == request.Id);


        if (existingRequest == null)
        {
            return;
        }


        existingRequest.Description =
            request.Description;


        context.SaveChanges();
    }


    public void DeleteRequest(int id)
    {
        var request =
            context.SpecialRequests
            .FirstOrDefault(x => x.Id == id);


        if (request != null)
        {
            context.SpecialRequests.Remove(request);
            context.SaveChanges();
        }
    }
}