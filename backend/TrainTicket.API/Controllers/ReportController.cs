using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportService service;

    public ReportController(ReportService service)
    {
        this.service = service;
    }


    [HttpGet("weekly")]
    public IActionResult GetWeeklyReport(DateTime date)
    {
        var result = service.GetWeeklyReport(date);
        return Ok(result);
    }
}