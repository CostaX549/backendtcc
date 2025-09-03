
using JwtAuthDotNet9.Models;

namespace JwtAuthDotNet9.Services;

public interface IVideoCallService
{
    Task<VideoCallDto> GetVideoCallByAppointmentIdAsync(Guid appointmentId);
    Task<VideoCallDto> ScheduleVideoCallAsync(Guid appointmentId, string meetingUrl);
}
