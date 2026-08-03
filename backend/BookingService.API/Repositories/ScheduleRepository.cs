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


    public List<Schedule> GetSchedules()
    {
        return context.Schedules
            .AsNoTracking()
            .ToList();
    }


    public Schedule? GetSchedule(int id)
    {
        return context.Schedules
            .FirstOrDefault(x => x.Id == id);
    }


    public Schedule CreateSchedule(Schedule schedule)
    {
        context.Schedules.Add(schedule);
        context.SaveChanges();

        return schedule;
    }


    public void UpdateSchedule(Schedule schedule)
    {
        var existingSchedule =
            context.Schedules
            .FirstOrDefault(x => x.Id == schedule.Id);


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


        context.SaveChanges();
    }


    public void DeleteSchedule(int id)
    {
        var schedule =
            context.Schedules
            .FirstOrDefault(x => x.Id == id);


        if (schedule != null)
        {
            context.Schedules.Remove(schedule);
            context.SaveChanges();
        }
    }
}