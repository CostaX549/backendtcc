namespace JwtAuthDotNet9.Entities;

public class MedicalRecord
{
    public Guid Id { get; set; }
    public Guid PsychologistId { get; set; }
    public PsychologistInformation Psychologist { get; set; } = null!;

    public Guid PatientId { get; set; }
    public User Patient { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Diagnosis { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public string? Recommendations { get; set; }

    public string? PdfUrl { get; set; }
}