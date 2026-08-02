using TrainTicket.API.Models;
using TrainTicket.API.Repositories;


namespace TrainTicket.API.Services;

public class SpecialRequestService
{
    private readonly IRepository<SpecialRequest> repository;


    public SpecialRequestService(
        IRepository<SpecialRequest> repository)
    {
        this.repository = repository;
    }


    public IEnumerable<SpecialRequest> GetRequests()
    {
        return repository.GetAll();
    }


    public SpecialRequest? GetRequest(int id)
    {
        return repository.GetById(id);
    }


    public SpecialRequest CreateRequest(
        SpecialRequest request)
    {
        repository.Add(request);

        return request;
    }


    public void UpdateRequest(
        SpecialRequest request)
    {
        repository.Update(request);
    }


    public void DeleteRequest(int id)
    {
        repository.Delete(id);
    }
}