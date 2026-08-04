using BookingService.API.Models;
using BookingService.API.Repositories;

namespace BookingService.API.Services;

public class SpecialRequestService
{
    private readonly SpecialRequestRepository repository;

    public SpecialRequestService(SpecialRequestRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<SpecialRequest>> GetRequests()
    {
        return await repository.GetRequests();
    }

    public async Task<SpecialRequest?> GetRequest(int id)
    {
        return await repository.GetRequest(id);
    }

    public async Task<SpecialRequest> CreateRequest(
        SpecialRequest request)
    {
        return await repository.CreateRequest(request);
    }

    public async Task UpdateRequest(
        SpecialRequest request)
    {
        await repository.UpdateRequest(request);
    }

    public async Task DeleteRequest(int id)
    {
        await repository.DeleteRequest(id);
    }
}