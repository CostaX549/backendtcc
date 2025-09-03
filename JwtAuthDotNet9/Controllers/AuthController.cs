using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthDotNet9.Data;
using JwtAuthDotNet9.Entities;
using JwtAuthDotNet9.Models;
using JwtAuthDotNet9.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace JwtAuthDotNet9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

      

      
    

        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserDto request)
        {
          var user = await _authService.RegisterAsync(request);
          if(user is null)
              return BadRequest("Email já existente");
          return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var result = await _authService.LoginAsync(request);
            if(result is null)
                return BadRequest("Email ou senha inválidos");
            return Ok(result);
        }
        
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser([FromServices] UserDbContext context)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token: missing user identifier.");

            var userId = Guid.Parse(userIdClaim.Value);

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return NotFound("User not found.");

            string? profilePictureUrl = null;
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                profilePictureUrl = $"{Request.Scheme}://{Request.Host}/uploads/{user.ProfilePictureUrl}";
            }
            var response = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.PhoneNumber,
                user.DeviceKey,
                user.Role,
                ProfilePicture = profilePictureUrl
            };

            // Se for um psicólogo, buscar e incluir informações extras
            if (user.Role == "Professional")
            {
                var psychologist = await context.PsychologistInformation.FirstOrDefaultAsync(p => p.UserId == userId);
                if (psychologist != null)
                {
                    return Ok(new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        psychologist.PhoneNumber,
                        user.DeviceKey,
                        user.Role,
                        psychologist.Description,
                        psychologist.ConsultationPrice,
                        psychologist.City,
                        psychologist.State,
                        psychologist.PostalCode,
                        psychologist.Address,
                        ProfilePicture = profilePictureUrl
                    });
                }
            }

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token");
            return Ok(result);
        }     
        [Authorize(Roles = "Professional")]
        [HttpPost("update-location")]
        public async Task<IActionResult> UpdateLocation([FromServices] UserDbContext context, [FromBody] LocationDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized("Invalid token");

            var userId = Guid.Parse(userIdClaim.Value);
            if (!Enum.TryParse<ConsultationType>(request.ConsultationType, true, out var consultationType))
            {
                return BadRequest("Tipo de consulta inválido. Deve ser: Híbrido, Presencial ou Online.");
            }
            // Buscar informações do psicólogo
            var psychologist = await context.PsychologistInformation.FirstOrDefaultAsync(p => p.UserId == userId);
            if (psychologist == null)
            {
                // Se não existir, criar novo registro
                psychologist = new PsychologistInformation
                {
                    UserId = userId,
                    State = request.State,
                    City = request.City,
                    PostalCode = request.CEP,
                    Address = request.StreetNumber,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Type = consultationType

                };
                context.PsychologistInformation.Add(psychologist);
            }
            else
            {
                // Atualizar registro existente
                psychologist.State = request.State;
                psychologist.City = request.City;
                psychologist.PostalCode = request.CEP;
                psychologist.Address = request.StreetNumber;
                psychologist.Latitude = request.Latitude;
                psychologist.Longitude = request.Longitude;
                psychologist.Type = consultationType;
            }

            await context.SaveChangesAsync();
            return Ok(new { message = "Localização salva com sucesso" });
        }
        [Authorize]
        [HttpGet("psychologist-info/{userId}")]
        public async Task<ActionResult<object>> GetPsychologistInfoByUserId([FromServices] UserDbContext context, Guid userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId && u.Role == "Professional")
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.Role,
                    ProfilePicture = string.IsNullOrEmpty(u.ProfilePictureUrl)
                        ? null
                        : $"{Request.Scheme}://{Request.Host}/{u.ProfilePictureUrl}",

                    PsychologistInfo = context.PsychologistInformation
                        .Where(p => p.UserId == u.Id)
                        .Select(p => new
                        {
                            p.Id,
                            p.UserId,
                            p.PhoneNumber,
                            p.Description,
                            p.ConsultationPrice,
                            p.City,
                            p.State,
                            p.Type,
                            p.PostalCode,
                            p.Latitude,
                            p.Longitude,
                            p.Address
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário não encontrado ou não é um psicólogo." });

            return Ok(user);
        }

// DTO
        public class LocationDto
        {
            public string State { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string CEP { get; set; } = string.Empty;
            public string StreetNumber { get; set; } = string.Empty;

            public double? Latitude { get; set; }
            public double? Longitude { get; set; }

            public string ConsultationType { get; set; } = string.Empty;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndPoint()
        {
            return Ok("Authenticated");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndPoint()
        {
            return Ok("You are a admin!");
        }

        [Authorize] // opcional: apenas admins podem listar psicólogos
        [HttpGet("psychologists")]
        public async Task<ActionResult<IEnumerable<object>>> GetPsychologists([FromServices] UserDbContext context)
        {
            var psychologists = await context.Users
                .Where(u => u.Role == "Professional")
                .OrderBy(u => u.Username) // pode ordenar como quiser
                .Take(20)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.DeviceKey,
                    u.Role,
                    ProfilePicture = string.IsNullOrEmpty(u.ProfilePictureUrl) 
                        ? null 
                        : $"{Request.Scheme}://{Request.Host}/{u.ProfilePictureUrl}",
                    PsychologistInfo = context.PsychologistInformation
                        .Where(p => p.UserId == u.Id)
                        .Select(p => new
                        {
                            p.PhoneNumber,
                            p.Description,
                            p.ConsultationPrice,
                            p.City,
                            p.State,
                            Type = p.Type.ToString(),
                            p.PostalCode,
                            p.Latitude,
                            p.Longitude,
                            p.Address
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(psychologists);
        }
    }
}
