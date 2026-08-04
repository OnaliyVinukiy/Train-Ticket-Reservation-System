using BookingService.API.Data;
using BookingService.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.API.Repositories;

public class ScheduleRepository
{
    private readonly AppDbContext context;

    public ScheduleRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Schedule>> GetSchedules()
    {
        return await context.Schedules
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Schedule?> GetSchedule(int id)
    {
        return await context.Schedules
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Schedule> CreateSchedule(Schedule schedule)
    {
        await context.Schedules.AddAsync(schedule);
        await context.SaveChangesAsync();

        return schedule;
    }

    public async Task UpdateSchedule(Schedule schedule)
    {
        var existingSchedule = await context.Schedules
            .FirstOrDefaultAsync(x => x.Id == schedule.Id);

        if (existingSchedule == null)
        {
            return;
        }

        existingSchedule.TravelDate =
            schedule.TravelDate;

        existingSchedule.DepartureTime =
            schedule.DepartureTime;

        existingSchedule.ArrivalTime =
            schedule.ArrivalTime;

        await context.SaveChangesAsync();
    }

    public async Task DeleteSchedule(int id)
    {
        var schedule = await context.Schedules
            .FirstOrDefaultAsync(x => x.Id == id);

        if (schedule != null)
        {
            context.Schedules.Remove(schedule);
            await context.SaveChangesAsync();
        }
    }
}