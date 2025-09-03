

namespace JwtAuthDotNet9.Entities
{
    public enum ConsultationType
    {
        Híbrido,
        Online,
        Presencial
    }

    public class PsychologistInformation
    {
        public Guid Id { get; set; }  

        public string OfficeName { get; set; } = string.Empty; 
        public string? Address { get; set; } = string.Empty; 
        public decimal ConsultationPrice { get; set; }
        public string? City { get; set; } = string.Empty; 
        public string? State { get; set; } = string.Empty; 
        public string? PostalCode { get; set; } = string.Empty; 
        public string PhoneNumber { get; set; } = string.Empty; 
        public string? Description { get; set; } 
        
        public double? Latitude { get; set; } 
        public double? Longitude { get; set; }

        // Novo campo para o tipo de atendimento
        public ConsultationType Type { get; set; }

        public ICollection<PsychologistRating> Ratings { get; set; } = new List<PsychologistRating>();

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;
    }
}