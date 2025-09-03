
namespace JwtAuthDotNet9.Models;


public class WorkingHourDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public List<BreakPeriodDto> Breaks { get; set; } = new();
}