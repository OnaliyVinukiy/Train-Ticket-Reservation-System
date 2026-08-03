using BookingService.API.Data;
using BookingService.API.Models;

namespace BookingService.API.Repositories;

public class ScheduleRepository
{
    private readonly AppDbContext context;

    public ScheduleRepository(AppDbContext context)
    {
        this.context = context;
    }

    public List<Schedule> GetSchedules()
    {
        return context.Schedules.ToList();
    }

    public Schedule? GetSchedule(int id)
    {
        return context.Schedules.Find(id);
    }

    public Schedule CreateSchedule(Schedule schedule)
    {
        context.Schedules.Add(schedule);
        context.SaveChanges();

        return schedule;
    }

    public void UpdateSchedule(Schedule schedule)
    {
        context.Schedules.Update(schedule);
        context.SaveChanges();
    }

    public void DeleteSchedule(int id)
    {
        var schedule = context.Schedules.Find(id);

        if (schedule != null)
        {
            context.Schedules.Remove(schedule);
            context.SaveChanges();
        }
    }
}