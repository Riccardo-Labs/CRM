using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentiController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/Agenti
        public async Task<ActionResult<IEnumerable<Agente>>> GetAgenti()
        {
            var agenti = await _context.Agenti.ToListAsync();
            return Ok(agenti);
        }

        [HttpGet("{id}")]
        // GET: api/Agenti/5
        public async Task<ActionResult<Agente>> GetAgente(int id)
        {
            var agente = await _context.Agenti.FindAsync(id);
            if (agente == null)
            {
                return NotFound();
            }
            return Ok(agente);
        }

        [HttpPost]
        // POST: api/Agenti
        public async Task<ActionResult<Agente>> PostAgente(Agente agente)
        {
            _context.Agenti.Add(agente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAgente), new { id = agente.IdAgente }, agente);
        }

        [HttpPut("{id}")]
        // PUT: api/Agenti/5
        public async Task<IActionResult> PutAgente(int id, Agente agente)
        {
            if (id != agente.IdAgente)
            {
                return BadRequest();
            }

            _context.Entry(agente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenteExists(id))
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

        [HttpDelete("{id}")]
        // DELETE: api/Agenti/5
        public async Task<IActionResult> DeleteAgente(int id)
        {
            var agente = await _context.Agenti.FindAsync(id);
            if (agente == null)
            {
                return NotFound();
            }

            _context.Agenti.Remove(agente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgenteExists(int id)
        {
            return _context.Agenti.Any(e => e.IdAgente == id);
        }
    }
}