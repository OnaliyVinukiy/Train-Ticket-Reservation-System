using TrainTicket.API.Models;
using TrainTicket.API.Repositories;


namespace TrainTicket.API.Services;


public class ScheduleService
{
    private readonly IRepository<Schedule> repository;


    public ScheduleService(
        IRepository<Schedule> repository)
    {
        this.repository = repository;
    }

    public IEnumerable<Schedule> GetSchedules()
    {
        return repository.GetAll();
    }

    public Schedule? GetSchedule(int id)
    {
        return repository.GetById(id);
    }

    public Schedule CreateSchedule(
        Schedule schedule)
    {
        repository.Add(schedule);

        return schedule;
    }

    public void UpdateSchedule(
        Schedule schedule)
    {
        repository.Update(schedule);
    }

    public void DeleteSchedule(int id)
    {
        repository.Delete(id);
    }
}