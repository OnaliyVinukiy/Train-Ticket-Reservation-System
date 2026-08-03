using Microsoft.AspNetCore.Mvc;
using PredictionService.API.DTOs;
using PredictionService.API.Services;

namespace PredictionService.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PredictionController : ControllerBase
{
    private readonly PredictionManagementService service;


    public PredictionController(
        PredictionManagementService service)
    {
        this.service = service;
    }


    [HttpGet]
    public IActionResult Predict(
        string route,
        DateTime travelDate,
        string departureTime)
    {
        PredictionResponseDto result =
            service.Predict(
                route,
                travelDate,
                departureTime);

        return Ok(result);
    }
}