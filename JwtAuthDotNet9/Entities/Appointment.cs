namespace JwtAuthDotNet9.Entities;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string Status { get; set; } = "Scheduled";

    public Guid PatientId { get; set; }
    public User Patient { get; set; } = null!;

    public Guid PsychologistId { get; set; }
    public PsychologistInformation Psychologist { get; set; } = null!;

    public string? Notes { get; set; }

    public VideoCall? VideoCall { get; set; }
}