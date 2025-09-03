using AutoMapper;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9.Services;

public class VideoCallService : IVideoCallService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public VideoCallService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VideoCallDto> GetVideoCallByAppointmentIdAsync(Guid appointmentId)
    {
        var videoCall = await _context.VideoCalls
            .FirstOrDefaultAsync(vc => vc.AppointmentId == appointmentId);
        return _mapper.Map<VideoCallDto>(videoCall);
    }

    public async Task<VideoCallDto> ScheduleVideoCallAsync(Guid appointmentId, string meetingUrl)
    {
        var videoCall = new VideoCall
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointmentId,
            MeetingUrl = meetingUrl,
            ScheduledAt = DateTime.UtcNow
        };
        _context.VideoCalls.Add(videoCall);
        await _context.SaveChangesAsync();
        return _mapper.Map<VideoCallDto>(videoCall);
    }
}
