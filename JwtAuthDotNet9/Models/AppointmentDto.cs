namespace JwtAuthDotNet9.Models;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Status { get; set; } = "Scheduled";
    public Guid PatientId { get; set; }
    public Guid PsychologistId { get; set; }
    public string? Notes { get; set; }
}