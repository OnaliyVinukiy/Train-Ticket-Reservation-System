using BookingService.API.Models;
using BookingService.API.Repositories;

namespace BookingService.API.Services;

public class ScheduleService
{
    private readonly ScheduleRepository repository;

    public ScheduleService(ScheduleRepository repository)
    {
        this.repository = repository;
    }

    public List<Schedule> GetSchedules()
    {
        return repository.GetSchedules();
    }

    public Schedule? GetSchedule(int id)
    {
        return repository.GetSchedule(id);
    }

    public Schedule CreateSchedule(Schedule schedule)
    {
        return repository.CreateSchedule(schedule);
    }

    public void UpdateSchedule(Schedule schedule)
    {
        repository.UpdateSchedule(schedule);
    }

    public void DeleteSchedule(int id)
    {
        repository.DeleteSchedule(id);
    }
}