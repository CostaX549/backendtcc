using AutoMapper;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9.Services;

public class ChatService : IChatService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public ChatService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(Guid userId1, Guid userId2)
    {
        var messages = await _context.ChatMessages
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                        (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ChatMessageDto>>(messages);
    }

    public async Task<ChatMessageDto> SendMessageAsync(CreateChatMessageDto dto)
    {
        var message = _mapper.Map<ChatMessage>(dto);
        message.Id = Guid.NewGuid();
        message.SentAt = DateTime.UtcNow;
        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();
        return _mapper.Map<ChatMessageDto>(message);
    }
}
