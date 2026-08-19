using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAttivitaController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/LogAttivita
        public async Task<ActionResult<IEnumerable<LogAttivita>>> GetLogAttivita()
        {
            return Ok(await _context.LogAttivita.ToListAsync());
        }

        [HttpGet("{id}")]
        // GET: api/LogAttivita/5
        public async Task<ActionResult<LogAttivita>> GetLogAttivitaById(int id)
        {
            var logAttivita = await _context.LogAttivita.FindAsync(id);

            if (logAttivita == null)
            {
                return NotFound();
            }

            return Ok(logAttivita);
        }

        [HttpPost]
        // POST: api/LogAttivita
        public async Task<ActionResult<LogAttivita>> PostLogAttivita(LogAttivita logAttivita)
        {
            var erroreFk = await ValidaForeignKeyAsync(logAttivita);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            _context.LogAttivita.Add(logAttivita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogAttivitaById), new { id = logAttivita.IdLogAttivita }, logAttivita);
        }

        [HttpPut("{id}")]
        // PUT: api/LogAttivita/5
        public async Task<IActionResult> PutLogAttivita(int id, LogAttivita logAttivita)
        {
            if (id != logAttivita.IdLogAttivita)
            {
                return BadRequest();
            }

            var erroreFk = await ValidaForeignKeyAsync(logAttivita);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            _context.Entry(logAttivita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogAttivitaExists(id))
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
        // DELETE: api/LogAttivita/5
        public async Task<IActionResult> DeleteLogAttivita(int id)
        {
            var logAttivita = await _context.LogAttivita.FindAsync(id);
            if (logAttivita == null)
            {
                return NotFound();
            }

            _context.LogAttivita.Remove(logAttivita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Metodo privato per validare le foreign key
        private async Task<string?> ValidaForeignKeyAsync(LogAttivita logAttivita)
        {
            if (!await _context.Agenti.AnyAsync(a => a.IdAgente == logAttivita.IdAgente))
            {
                return "L'agente associato non esiste.";
            }

            if (logAttivita.IdOrdine != null &&
                !await _context.Ordini.AnyAsync(o => o.IdOrdine == logAttivita.IdOrdine))
            {
                return "L'ordine associato non esiste.";
            }

            if (logAttivita.IdContatto != null &&
                !await _context.Contatti.AnyAsync(c => c.IdContatto == logAttivita.IdContatto))
            {
                return "Il contatto associato non esiste.";
            }

            return null;
        }

        private bool LogAttivitaExists(int id)
        {
            return _context.LogAttivita.Any(e => e.IdLogAttivita == id);
        }
    }
}
