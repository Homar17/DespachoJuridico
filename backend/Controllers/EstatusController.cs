using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatusController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estatus>>> GetEstatus()
        {
            return await _context.Estatus.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Estatus>> PostEstatus(Estatus estatus)
        {
            _context.Estatus.Add(estatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstatus), new { id = estatus.ID }, estatus);
        }
    }
}