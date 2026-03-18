using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contraseña == request.Contraseña);

            if (usuario == null)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("Nombre", usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol?.Descripcion ?? "Cliente")
            };

            var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return Ok(new
            {
                mensaje = "¡Login exitoso!",
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }

    public class LoginRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
    }
}