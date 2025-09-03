using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetById(Guid id) =>
        Ok(await appointmentService.GetAppointmentByIdAsync(id));

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByUser(Guid userId) =>
        Ok(await appointmentService.GetAppointmentsByUserIdAsync(userId));

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto dto) =>
        Ok(await appointmentService.CreateAppointmentAsync(dto));

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id) =>
        await appointmentService.CancelAppointmentAsync(id) ? NoContent() : NotFound();
}