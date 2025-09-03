namespace JwtAuthDotNet9.Models;

public class PsychologistInformationDto
{
    public Guid Id { get; set; }  
    public string OfficeName { get; set; } = string.Empty;  
    public string? Address { get; set; }  
    public string? City { get; set; }  
    public string? State { get; set; }  
    public string? PostalCode { get; set; }  
    public string PhoneNumber { get; set; } = string.Empty;  
    public string? Description { get; set; }  
    public decimal ConsultationPrice { get; set; }  
}