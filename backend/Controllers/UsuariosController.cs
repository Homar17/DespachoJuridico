using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; 

        public UsuariosController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // get: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios
                                 .Include(u => u.Rol)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                                        .Include(u => u.Rol)
                                        .FirstOrDefaultAsync(u => u.ID == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var rolExiste = await _context.Roles.AnyAsync(r => r.ID == usuario.RolId);
            if (!rolExiste)
            {
                return BadRequest("El Rol especificado no existe.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            _ = _emailService.EnviarCorreoBienvenidaAsync(usuario.Correo, usuario.Nombre);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.ID }, usuario);
        }
    }
}