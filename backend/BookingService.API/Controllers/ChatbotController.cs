using Microsoft.AspNetCore.Mvc;
using TrainTicket.API.Models.DTOs;
using TrainTicket.API.Services;

namespace TrainTicket.API.Controllers;

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