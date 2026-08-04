using Microsoft.AspNetCore.Mvc;
using BookingService.API.Models;
using BookingService.API.Services;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ScheduleService service;


    public ScheduleController(
        ScheduleService service)
    {
        this.service = service;
    }


    [HttpGet]
    public async Task<IActionResult> GetSchedules()
    {
        var schedules = await service.GetSchedules();

        return Ok(schedules);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetSchedule(int id)
    {
        var schedule = await service.GetSchedule(id);


        if (schedule == null)
        {
            return NotFound(new
            {
                message = "Schedule not found"
            });
        }


        return Ok(schedule);
    }


    [HttpPost]
    public async Task<IActionResult> CreateSchedule(
        Schedule schedule)
    {
        var created =
            await service.CreateSchedule(schedule);


        return Ok(created);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(
        int id,
        Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return BadRequest(new
            {
                message = "Schedule ID mismatch"
            });
        }


        var existing =
            await service.GetSchedule(id);


        if (existing == null)
        {
            return NotFound(new
            {
                message = "Schedule not found"
            });
        }


        await service.UpdateSchedule(schedule);


        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule =
            await service.GetSchedule(id);


        if (schedule == null)
        {
            return NotFound(new
            {
                message = "Schedule not found"
            });
        }


        await service.DeleteSchedule(id);


        return NoContent();
    }
}