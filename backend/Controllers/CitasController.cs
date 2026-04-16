using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using backend.Services; // Importamos la carpeta de servicios

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Inyectamos el servicio

        // Actualizamos el constructor para recibir el IEmailService
        public CitasController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

            // 1. Guardamos la cita en la base de datos
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            // 2. Traemos los datos del usuario para el correo
            var usuario = await _context.Usuarios.FindAsync(cita.id_user);
            
            if (usuario != null)
            {
                // Ajustamos la fecha a la zona horaria local (UTC-6) para el texto del correo
                var fechaLocal = cita.FechaHora.AddHours(-6);

                // Enviamos el correo en segundo plano para no hacer esperar al frontend
                _ = _emailService.EnviarCorreoConfirmacionAsync(
                    usuario.Correo, // <-- AQUÍ ESTÁ EL CAMBIO
                    usuario.Nombre, 
                    fechaLocal.ToString("dd/MM/yyyy"), 
                    fechaLocal.ToString("HH:mm")
                );
            }

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

        //GET: Se obtienen las fechas y horas disponibles para citas
        [AllowAnonymous]
        [HttpGet("disponibilidad/{fecha}")]
        public async Task<IActionResult> GetHorasDisponibles(string fecha)
        {
            if (!DateTime.TryParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaConsulta))
            {
                return BadRequest($"Error de formato. C# recibió: {fecha}");
            }

            // 1. Establecer límites en base a Guadalajara (UTC-6)
            var inicioDiaUtc = DateTime.SpecifyKind(fechaConsulta.Date, DateTimeKind.Utc).AddHours(6);
            var finDiaUtc = inicioDiaUtc.AddDays(1);

            var citasDelDia = await _context.Citas
                .Where(c => c.FechaHora >= inicioDiaUtc && c.FechaHora < finDiaUtc)
                .ToListAsync();

            // 2. Extraer horas restando las 6 horas de UTC para compararlo con tu horario local
            var horasOcupadas = citasDelDia
                .Select(c => c.FechaHora.AddHours(-6).TimeOfDay)
                .ToList();

            var horasLaborales = new List<TimeSpan>();
            for (int i = 9; i <= 18; i++)
            {
                horasLaborales.Add(TimeSpan.FromHours(i));
            }

            var horasDisponibles = horasLaborales
                .Where(h => !horasOcupadas.Contains(h))
                .Select(h => h.ToString(@"hh\:mm")) 
                .ToList();

            return Ok(horasDisponibles);
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