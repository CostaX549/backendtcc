namespace JwtAuthDotNet9.Entities;

public class BreakPeriod
{
    public Guid Id { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }

    public Guid WorkingHourId { get; set; }
    public WorkingHour WorkingHour { get; set; } = null!;
}