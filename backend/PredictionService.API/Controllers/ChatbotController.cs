using Microsoft.AspNetCore.Mvc;
using PredictionService.API.DTOs;
using PredictionService.API.Services;

namespace PredictionService.API.Controllers;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly ChatbotService service;

    public ChatbotController(ChatbotService service)
    {
        this.service = service;
    }

    [HttpPost]
    public IActionResult Chat(ChatbotRequestDto request)
    {
        var response = service.ProcessMessage(request.Message);
        return Ok(response);
    }
}