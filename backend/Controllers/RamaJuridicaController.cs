using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamaJuridicaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RamaJuridicaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RamaJuridica>>> GetRamas()
        {
            return await _context.RamasJuridicas.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<RamaJuridica>> PostRama(RamaJuridica rama)
        {
            _context.RamasJuridicas.Add(rama);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRamas), new { id = rama.ID }, rama);
        }
    }
}