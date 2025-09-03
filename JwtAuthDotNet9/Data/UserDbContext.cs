using JwtAuthDotNet9.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNet9.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<PsychologistInformation> PsychologistInformation { get; set; }
    public DbSet<WorkingHour> WorkingHours { get; set; }
    public DbSet<BreakPeriod> BreakPeriods { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<VideoCall> VideoCalls { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<PsychologistRating> PsychologistRatings { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1:1 User <-> PsychologistInformation
        modelBuilder.Entity<PsychologistInformation>()
            .HasOne(p => p.User)
            .WithOne(u => u.PsychologistInformation)
            .HasForeignKey<PsychologistInformation>(p => p.UserId);

        // 1:N PsychologistInformation <-> WorkingHour
        modelBuilder.Entity<WorkingHour>()
            .HasOne(w => w.Psychologist)
            .WithMany()
            .HasForeignKey(w => w.PsychologistId);

        // 1:N WorkingHour <-> BreakPeriod
        modelBuilder.Entity<BreakPeriod>()
            .HasOne(b => b.WorkingHour)
            .WithMany(w => w.Breaks)
            .HasForeignKey(b => b.WorkingHourId);

        // Appointment: User(Patient) <-> Psychologist
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Psychologist)
            .WithMany()
            .HasForeignKey(a => a.PsychologistId)
            .OnDelete(DeleteBehavior.Restrict);

        // Appointment <-> VideoCall (1:1)
        modelBuilder.Entity<VideoCall>()
            .HasOne(v => v.Appointment)
            .WithOne(a => a.VideoCall)
            .HasForeignKey<VideoCall>(v => v.AppointmentId);

        // Chat: Sender -> Receiver
        modelBuilder.Entity<ChatMessage>()
            .HasOne(c => c.Sender)
            .WithMany()
            .HasForeignKey(c => c.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(c => c.Receiver)
            .WithMany()
            .HasForeignKey(c => c.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // MedicalRecord
        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Patient)
            .WithMany()
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Psychologist)
            .WithMany()
            .HasForeignKey(m => m.PsychologistId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PsychologistRating>()
            .HasOne(r => r.Psychologist)
            .WithMany(p => p.Ratings)
            .HasForeignKey(r => r.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistRating>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
