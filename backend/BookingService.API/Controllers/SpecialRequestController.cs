using Microsoft.AspNetCore.Mvc;
using BookingService.API.Models;
using BookingService.API.Services;

namespace BookingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialRequestController : ControllerBase
{
    private readonly SpecialRequestService service;


    public SpecialRequestController(
        SpecialRequestService service)
    {
        this.service = service;
    }


    [HttpGet]
    public async Task<IActionResult> GetRequests()
    {
        var requests = await service.GetRequests();

        return Ok(requests);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        var request =
            await service.GetRequest(id);


        if (request == null)
        {
            return NotFound(new
            {
                message = "Special request not found"
            });
        }


        return Ok(request);
    }


    [HttpPost]
    public async Task<IActionResult> CreateRequest(
        SpecialRequest request)
    {
        var created =
            await service.CreateRequest(request);


        return Ok(created);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequest(
        int id,
        SpecialRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new
            {
                message = "Request ID mismatch"
            });
        }


        var existing =
            await service.GetRequest(id);


        if (existing == null)
        {
            return NotFound(new
            {
                message = "Special request not found"
            });
        }


        await service.UpdateRequest(request);


        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var request =
            await service.GetRequest(id);


        if (request == null)
        {
            return NotFound(new
            {
                message = "Special request not found"
            });
        }


        await service.DeleteRequest(id);


        return NoContent();
    }
}