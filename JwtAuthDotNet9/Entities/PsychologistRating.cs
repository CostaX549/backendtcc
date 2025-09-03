namespace JwtAuthDotNet9.Entities
{
    public class PsychologistRating
    {
        public Guid Id { get; set; }

        public int Rating { get; set; } // De 1 a 5
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacionamento com o psicólogo
        public Guid PsychologistId { get; set; }
        public PsychologistInformation Psychologist { get; set; } = null!;

        // Relacionamento com o usuário que fez a avaliação
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
