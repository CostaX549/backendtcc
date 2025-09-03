namespace JwtAuthDotNet9.Models;

public class VideoCallDto
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string MeetingUrl { get; set; } = string.Empty;
    public Guid AppointmentId { get; set; }
}