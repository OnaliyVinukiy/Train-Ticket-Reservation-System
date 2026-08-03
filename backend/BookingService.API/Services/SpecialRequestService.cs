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

    public List<SpecialRequest> GetRequests()
    {
        return repository.GetRequests();
    }

    public SpecialRequest? GetRequest(int id)
    {
        return repository.GetRequest(id);
    }

    public SpecialRequest CreateRequest(SpecialRequest request)
    {
        return repository.CreateRequest(request);
    }

    public void UpdateRequest(SpecialRequest request)
    {
        repository.UpdateRequest(request);
    }

    public void DeleteRequest(int id)
    {
        repository.DeleteRequest(id);
    }
}