namespace JwtAuthDotNet9.Entities;

public class WorkingHour
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public List<BreakPeriod> Breaks { get; set; } = new();

    public Guid PsychologistId { get; set; }
    public PsychologistInformation Psychologist { get; set; } = null!;
}