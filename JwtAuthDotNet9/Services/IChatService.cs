using JwtAuthDotNet9.Models;

namespace JwtAuthDotNet9.Services;

public interface IChatService
{
    Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid userId1, Guid userId2);
    Task<ChatMessageDto> SendMessageAsync(CreateChatMessageDto dto);
}