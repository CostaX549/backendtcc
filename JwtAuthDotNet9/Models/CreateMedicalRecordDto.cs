




public class CreateMedicalRecordDto
{
    public Guid PsychologistId { get; set; }
    public Guid PatientId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public string? Recommendations { get; set; }
}