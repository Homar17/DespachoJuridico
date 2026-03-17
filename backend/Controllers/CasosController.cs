using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CasosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Casos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caso>>> GetCasos()
        {
            return await _context.Casos
                .Include(c => c.Usuario)
                .Include(c => c.Estatus)
                .Include(c => c.RamaJuridica)
                .ToListAsync();
        }

        // GET: api/Casos
        [HttpGet("{id}")]
        public async Task<ActionResult<Caso>> GetCaso(int id)
        {
            var caso = await _context.Casos
                .Include(c => c.Usuario)
                .Include(c => c.Estatus)
                .Include(c => c.RamaJuridica)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (caso == null) return NotFound();

            return caso;
        }

        // POST: api/Casos
        [HttpPost]
        public async Task<ActionResult<Caso>> PostCaso(Caso caso)
        {
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.ID == caso.id_usuario);
            var estatusExiste = await _context.Estatus.AnyAsync(e => e.ID == caso.Id_estatus);
            var ramaExiste = await _context.RamasJuridicas.AnyAsync(r => r.ID == caso.Id_RamaJuridica);

            if (!usuarioExiste || !estatusExiste || !ramaExiste)
            {
                return BadRequest("El Usuario, Estatus o Rama Jurídica especificados no existen.");
            }

            _context.Casos.Add(caso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCaso), new { id = caso.ID }, caso);
        }

        // PUT: api/Casos/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaso(int id, Caso caso)
        {
            if (id != caso.ID) return BadRequest("El ID de la URL no coincide con el del Caso.");

            _context.Entry(caso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasoExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Casos/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaso(int id)
        {
            var caso = await _context.Casos.FindAsync(id);
            if (caso == null) return NotFound();

            _context.Casos.Remove(caso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CasoExists(int id)
        {
            return _context.Casos.Any(e => e.ID == id);
        }
    }
}