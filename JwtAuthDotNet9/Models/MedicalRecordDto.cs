namespace JwtAuthDotNet9.Models;

public class MedicalRecordDto
{
    public Guid Id { get; set; }
    public Guid PsychologistId { get; set; }
    public Guid PatientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
    public string? PdfUrl { get; set; }
}