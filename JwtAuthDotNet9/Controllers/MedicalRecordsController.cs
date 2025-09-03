using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MedicalRecordsController(IMedicalRecordService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalRecordDto>> GetById(Guid id) =>
        Ok(await service.GetMedicalRecordByIdAsync(id));

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetByPatient(Guid patientId) =>
        Ok(await service.GetMedicalRecordsByPatientIdAsync(patientId));

    [HttpPost]
    public async Task<ActionResult<MedicalRecordDto>> Create([FromBody] CreateMedicalRecordDto dto) =>
        Ok(await service.CreateMedicalRecordAsync(dto));
}