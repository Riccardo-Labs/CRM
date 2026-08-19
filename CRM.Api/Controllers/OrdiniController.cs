using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/Ordini
        public async Task<ActionResult<IEnumerable<Ordine>>> GetOrdini()
        {
            return Ok(await _context.Ordini.ToListAsync());
        }

        [HttpGet("{id}")]
        // GET: api/Ordini/5
        public async Task<ActionResult<Ordine>> GetOrdine(int id)
        {
            var ordine = await _context.Ordini.FindAsync(id);

            if (ordine == null)
            {
                return NotFound();
            }

            return Ok(ordine);
        }

        [HttpPost]
        // POST: api/Ordini
        public async Task<ActionResult<Ordine>> PostOrdine(Ordine ordine)
        {
            var erroreFk = await ValidaForeignKeyAsync(ordine);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrdine), new { id = ordine.IdOrdine }, ordine);
        }

        [HttpPut("{id}")]
        // PUT: api/Ordini/5
        public async Task<IActionResult> PutOrdine(int id, Ordine ordine)
        {
            if (id != ordine.IdOrdine)
            {
                return BadRequest();
            }

            var erroreFk = await ValidaForeignKeyAsync(ordine);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            var statoAttuale = await _context.Ordini
                .Where(o => o.IdOrdine == id)
                .Select(o => o.Stato)
                .FirstOrDefaultAsync();

            if (statoAttuale == null)
            {
                return NotFound();
            }

            var statiChiusi = new[] { "Vinto", "Perso" };
            if (statiChiusi.Contains(statoAttuale) && ordine.Stato != statoAttuale)
            {
                return BadRequest($"L'ordine è già chiuso (stato: {statoAttuale}): non è possibile modificarne lo stato.");
            }

            _context.Entry(ordine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdineExists(id))
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
        // DELETE: api/Ordini/5
        public async Task<IActionResult> DeleteOrdine(int id)
        {
            var ordine = await _context.Ordini.FindAsync(id);
            if (ordine == null)
            {
                return NotFound();
            }

            _context.Ordini.Remove(ordine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> ValidaForeignKeyAsync(Ordine ordine)
        {
            if (!await _context.Agenti.AnyAsync(a => a.IdAgente == ordine.IdAgente))
            {
                return "L'agente associato non esiste.";
            }

            if (!await _context.AziendaClienti.AnyAsync(a => a.IdAziendaCliente == ordine.IdAziendaCliente))
            {
                return "L'azienda cliente associata non esiste.";
            }

            if (ordine.IdContattoRiferimento != null &&
                !await _context.Contatti.AnyAsync(c => c.IdContatto == ordine.IdContattoRiferimento))
            {
                return "Il contatto di riferimento associato non esiste.";
            }

            return null;
        }

        private bool OrdineExists(int id)
        {
            return _context.Ordini.Any(e => e.IdOrdine == id);
        }
    }
}