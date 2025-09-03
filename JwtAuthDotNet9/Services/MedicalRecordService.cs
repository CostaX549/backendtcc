using AutoMapper;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public MedicalRecordService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicalRecordDto> GetMedicalRecordByIdAsync(Guid id)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        return _mapper.Map<MedicalRecordDto>(record);
    }

    public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientIdAsync(Guid patientId)
    {
        var records = await _context.MedicalRecords
            .Where(r => r.PatientId == patientId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
    }

    public async Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto dto)
    {
        var record = _mapper.Map<MedicalRecord>(dto);
        record.Id = Guid.NewGuid();
        record.CreatedAt = DateTime.UtcNow;
        _context.MedicalRecords.Add(record);
        await _context.SaveChangesAsync();
        return _mapper.Map<MedicalRecordDto>(record);
    }
}
