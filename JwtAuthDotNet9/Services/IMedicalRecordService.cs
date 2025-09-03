using JwtAuthDotNet9.Models;

public interface IMedicalRecordService
{
    Task<MedicalRecordDto> GetMedicalRecordByIdAsync(Guid id);
    Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientIdAsync(Guid patientId);
    Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto dto);
}