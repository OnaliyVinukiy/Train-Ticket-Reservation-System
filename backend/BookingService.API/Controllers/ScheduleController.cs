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
    public IActionResult GetSchedules()
    {
        return Ok(service.GetSchedules());
    }


    [HttpGet("{id}")]
    public IActionResult GetSchedule(int id)
    {
        var schedule = service.GetSchedule(id);


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
    public IActionResult CreateSchedule(
        Schedule schedule)
    {
        return Ok(
            service.CreateSchedule(schedule)
        );
    }


    [HttpPut("{id}")]
    public IActionResult UpdateSchedule(
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


        var existing = service.GetSchedule(id);


        if (existing == null)
        {
            return NotFound(new
            {
                message = "Schedule not found"
            });
        }


        service.UpdateSchedule(schedule);


        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteSchedule(int id)
    {
        var schedule = service.GetSchedule(id);


        if (schedule == null)
        {
            return NotFound(new
            {
                message = "Schedule not found"
            });
        }


        service.DeleteSchedule(id);


        return NoContent();
    }
}