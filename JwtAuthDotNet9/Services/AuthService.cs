using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
namespace JwtAuthDotNet9.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext context;
        private readonly IConfiguration configuration;

   
        public AuthService(UserDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

      
        public async Task<TokenResponseDto?> RegisterAsync([FromForm] UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username) || await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null; 
            }
            
            var validRoles = new[] { "User", "Admin", "Professional" };
            var roleToAssign = validRoles.Contains(request.Role) ? request.Role : "User";

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = roleToAssign
            };

         
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHash = hashedPassword;
            if (request.ProfilePicture != null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfilePicture.FileName)}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/uploads/{fileName}";
            }

           
            context.Users.Add(user);
            await context.SaveChangesAsync();
            if (roleToAssign == "Professional")
            {
                var psychologist = new PsychologistInformation
                {
                    UserId = user.Id,
                    OfficeName = request.Username, 
                    ConsultationPrice = request.ConsultationPrice,
                    PhoneNumber = request.PhoneNumber
                };

                context.PsychologistInformation.Add(psychologist);
                await context.SaveChangesAsync();
            }
            return await CreateTokenResponse(user); 
        }

     
        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return null; 
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null; 
            }

            var response = await CreateTokenResponse(user);
            return response; 
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            var response = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
            return response;
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;
            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return refreshToken;
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

        
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

           
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

          
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
