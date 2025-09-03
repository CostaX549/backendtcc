using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VideoCallsController(IVideoCallService videoCallService) : ControllerBase
{
    [HttpGet("appointment/{appointmentId}")]
    public async Task<ActionResult<VideoCallDto>> GetByAppointment(Guid appointmentId) =>
        Ok(await videoCallService.GetVideoCallByAppointmentIdAsync(appointmentId));

    [HttpPost]
    public async Task<ActionResult<VideoCallDto>> Schedule([FromBody] CreateVideoCallDto dto) =>
        Ok(await videoCallService.ScheduleVideoCallAsync(dto.AppointmentId, dto.MeetingUrl));
}