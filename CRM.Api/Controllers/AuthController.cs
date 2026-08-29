using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CRM.Data.Models;
using CRM.Api.DTOs;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(CrmContext context, IConfiguration configuration) : ControllerBase
    {
        private readonly CrmContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("login")]
        [AllowAnonymous]
        // POST: api/Auth/login
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto login)
        {
            var utente = await _context.Utenti
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Attivo);

            if (utente == null)
            {
                return Unauthorized("Credenziali non valide.");
            }

            var hasher = new PasswordHasher<Utente>();
            var risultato = hasher.VerifyHashedPassword(utente, utente.PasswordHash, login.Password);

            if (risultato == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Credenziali non valide.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utente.IdUtente.ToString()),
                new Claim(ClaimTypes.Email, utente.Email),
                new Claim(ClaimTypes.Role, utente.Ruolo)
            };

            var chiave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenziali = new SigningCredentials(chiave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                signingCredentials: credenziali
            );

            var risposta = new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = utente.Email,
                Ruolo = utente.Ruolo
            };

            return Ok(risposta);
        }
    }
}
