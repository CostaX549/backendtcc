namespace JwtAuthDotNet9.Models;

public class CreateAppointmentDto
{
    public DateTime AppointmentDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public Guid PatientId { get; set; }
    public Guid PsychologistId { get; set; }
    public string? Notes { get; set; }
}