using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatsController(IChatService chatService) : ControllerBase
{
    [HttpGet("{userId1}/{userId2}")]
    public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetHistory(Guid userId1, Guid userId2) =>
        Ok(await chatService.GetChatHistoryAsync(userId1, userId2));

    [HttpPost]
    public async Task<ActionResult<ChatMessageDto>> Send([FromBody] CreateChatMessageDto dto) =>
        Ok(await chatService.SendMessageAsync(dto));
}