namespace JwtAuthDotNet9.Models;
public class BreakPeriodDto
{
    public Guid Id { get; set; }
    public TimeSpan Start { get; set; }
    public TimeSpan End { get; set; }
}