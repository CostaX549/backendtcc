namespace JwtAuthDotNet9.Entities;

public class VideoCall
{
    public Guid Id { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string MeetingUrl { get; set; } = string.Empty;

    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
}