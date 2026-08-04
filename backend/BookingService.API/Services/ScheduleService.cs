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

    public async Task<List<Schedule>> GetSchedules()
    {
        return await repository.GetSchedules();
    }

    public async Task<Schedule?> GetSchedule(int id)
    {
        return await repository.GetSchedule(id);
    }

    public async Task<Schedule> CreateSchedule(Schedule schedule)
    {
        return await repository.CreateSchedule(schedule);
    }

    public async Task UpdateSchedule(Schedule schedule)
    {
        await repository.UpdateSchedule(schedule);
    }

    public async Task DeleteSchedule(int id)
    {
        await repository.DeleteSchedule(id);
    }
}