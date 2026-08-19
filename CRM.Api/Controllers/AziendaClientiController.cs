using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AziendaClientiController : ControllerBase
    {
        private readonly CrmContext _context;
        public AziendaClientiController(CrmContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: api/AziendaClienti
        public async Task<ActionResult<IEnumerable<AziendaCliente>>> GetAllAziendeClienti()
        {
            var aziendeClienti = await _context.AziendaClienti.ToListAsync();
            return Ok(aziendeClienti);
        }

        [HttpGet("{id}")]
        // GET: api/AziendaClienti/5
        public async Task<ActionResult<AziendaCliente>> GetAziendaCliente(int id)
        {
            var aziendaCliente = await _context.AziendaClienti.FindAsync(id);
            if (aziendaCliente == null)
            {
                return NotFound();
            }
            return Ok(aziendaCliente);
        }

        [HttpPost]
        // POST: api/AziendaClienti
        public async Task<ActionResult<AziendaCliente>> PostAziendaCliente(AziendaCliente aziendaCliente)
        {
            _context.AziendaClienti.Add(aziendaCliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAziendaCliente), new { id = aziendaCliente.IdAziendaCliente }, aziendaCliente);
        }

        [HttpPut("{id}")]
        // PUT: api/AziendaClienti/5
        public async Task<IActionResult> PutAziendaCliente(int id, AziendaCliente aziendaCliente)
        {
            if (id != aziendaCliente.IdAziendaCliente)
            {
                return BadRequest();
            }

            _context.Entry(aziendaCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AziendaClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AziendaClienteExists(int id)
        {
            return _context.AziendaClienti.Any(e => e.IdAziendaCliente == id);
        }
    }
}