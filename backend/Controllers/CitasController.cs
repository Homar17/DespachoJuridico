using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            return await _context.Citas
                                 .Include(c => c.Usuario)
                                 .ToListAsync();
        }

        // GET: api/Citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas
                                     .Include(c => c.Usuario)
                                     .FirstOrDefaultAsync(c => c.ID == id);

            if (cita == null) return NotFound();

            return cita;
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita(Cita cita)
        {
            if (cita.FechaHora.Minute != 0 || cita.FechaHora.Second != 0)
            {
                return BadRequest("Las citas solo pueden agendarse en horas cerradas (ejemplo: 11:00, 14:00). Por favor, ajusta los minutos a 00.");
            }

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.ID == cita.id_user);
            if (!usuarioExiste)
            {
                return BadRequest("El Usuario especificado no existe en el sistema.");
            }

            var fechaFinNuevaCita = cita.FechaHora.AddHours(1);

            var horarioOcupado = await _context.Citas.AnyAsync(c =>
                cita.FechaHora < c.FechaHora.AddHours(1) &&
                fechaFinNuevaCita > c.FechaHora
            );

            if (horarioOcupado)
            {
                return Conflict("Este horario ya está reservado por otro cliente. Por favor, elige una hora diferente.");
            }

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCita), new { id = cita.ID }, cita);
        }

        // PUT: api/Citas/5 (Para reprogramar la fecha o cambiar la descripción)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.ID) return BadRequest("El ID de la URL no coincide.");

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CitaExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Citas/5 (Para cancelar citas)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CitaExists(int id)
        {
            return _context.Citas.Any(e => e.ID == id);
        }
    }
}