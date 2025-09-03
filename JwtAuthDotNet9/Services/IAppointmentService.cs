using JwtAuthDotNet9.Models;

namespace JwtAuthDotNet9.Services;


public interface IAppointmentService
{
    Task<AppointmentDto> GetAppointmentByIdAsync(Guid id);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByUserIdAsync(Guid userId);
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto dto);
    Task<bool> CancelAppointmentAsync(Guid id);
}