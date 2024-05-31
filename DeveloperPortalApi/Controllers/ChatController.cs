using DeveloperPortalApi.Data;
using DeveloperPortalApi.Entities;
using DeveloperPortalApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveloperPortalApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ChatController(ApplicationDbContext context, ChatService chatService) : ControllerBase
{
    // Get all messages
    [HttpGet]
    public async Task<ActionResult<List<ChatMessage>>> GetAllMessages()
    {
        var messages = await context.ChatMessages.ToListAsync();
        return messages;
    }

    [HttpPost]
    public async Task<IActionResult> PostMessage(ChatMessage message)
    {
        if (string.IsNullOrEmpty(message.Username) || string.IsNullOrEmpty(message.Message))
        {
            return BadRequest("Username and message are required.");
        }

        await chatService.AddMessage(message);

        return Ok();
    }
}
