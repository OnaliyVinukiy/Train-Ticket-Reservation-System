using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models.DTOs;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly ChatbotService service;

    public ChatbotController(ChatbotService service)
    {
        this.service = service;
    }

    // Chatbot question endpoint
    [HttpPost]
    public IActionResult Ask(ChatbotRequestDto request)
    {
        var response = service.Ask(request.Question);
        return Ok(response);
    }

    // Seat availability prediction
    [HttpGet("availability")]
    public IActionResult PredictAvailability(DateTime date, string departure, string destination)
    {
        var result = service.PredictAvailability(date, departure, destination);
        return Ok(result);
    }

    // Ticket price prediction
    [HttpGet("price")]
    public IActionResult PredictPrice()
    {
        var result = service.PredictPrice();
        return Ok(result);
    }
}