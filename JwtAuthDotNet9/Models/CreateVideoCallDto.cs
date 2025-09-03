namespace JwtAuthDotNet9.Models
{
    public class CreateVideoCallDto
    {
        public Guid AppointmentId { get; set; }
        public string MeetingUrl { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
    }
}