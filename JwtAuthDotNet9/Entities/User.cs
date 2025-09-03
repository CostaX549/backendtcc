namespace JwtAuthDotNet9.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? RefreshToken { get; set; }
        public string? DeviceKey { get; set; }
        public string? ProfilePictureUrl { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public PsychologistInformation? PsychologistInformation { get; set; }
}