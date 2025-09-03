namespace JwtAuthDotNet9.Models;

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;    
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal ConsultationPrice { get; set; }
    public IFormFile? ProfilePicture { get; set; }

}