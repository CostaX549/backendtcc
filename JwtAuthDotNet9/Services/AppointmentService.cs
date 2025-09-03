using AutoMapper;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9.Services;

public class AppointmentService : IAppointmentService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public AppointmentService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByUserIdAsync(Guid userId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.PatientId == userId || a.Psychologist.UserId == userId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var appointment = _mapper.Map<Appointment>(dto);
        appointment.Id = Guid.NewGuid();
        appointment.Status = "Scheduled";
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<bool> CancelAppointmentAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return false;
        appointment.Status = "Canceled";
        await _context.SaveChangesAsync();
        return true;
    }
}
