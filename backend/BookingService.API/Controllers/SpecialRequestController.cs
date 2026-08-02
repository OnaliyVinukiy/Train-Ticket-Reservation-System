using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models;
using TrainTicket.API.Services;


namespace TrainTicket.API.Controllers;


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
    public IActionResult GetRequests()
    {
        return Ok(service.GetRequests());
    }


    [HttpGet("{id}")]
    public IActionResult GetRequest(int id)
    {
        var request = service.GetRequest(id);


        if (request == null)
        {
            return NotFound();
        }


        return Ok(request);
    }


    [HttpPost]
    public IActionResult CreateRequest(
        SpecialRequest request)
    {
        return Ok(
            service.CreateRequest(request)
        );
    }


    [HttpPut("{id}")]
    public IActionResult UpdateRequest(
        int id,
        SpecialRequest request)
    {

        if (id != request.Id)
        {
            return BadRequest();
        }


        service.UpdateRequest(request);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteRequest(int id)
    {
        service.DeleteRequest(id);

        return NoContent();
    }
}