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

    public async Task<List<SpecialRequest>> GetRequests()
    {
        return await context.SpecialRequests
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<SpecialRequest?> GetRequest(int id)
    {
        return await context.SpecialRequests
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SpecialRequest> CreateRequest(SpecialRequest request)
    {
        await context.SpecialRequests.AddAsync(request);
        await context.SaveChangesAsync();

        return request;
    }

    public async Task UpdateRequest(SpecialRequest request)
    {
        var existingRequest = await context.SpecialRequests
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (existingRequest == null)
        {
            return;
        }

        existingRequest.Description =
            request.Description;

        await context.SaveChangesAsync();
    }

    public async Task DeleteRequest(int id)
    {
        var request = await context.SpecialRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (request != null)
        {
            context.SpecialRequests.Remove(request);
            await context.SaveChangesAsync();
        }
    }
}