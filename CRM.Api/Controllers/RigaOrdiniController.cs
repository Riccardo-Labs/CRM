using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RigaOrdiniController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/RigaOrdini
        public async Task<ActionResult<IEnumerable<RigaOrdine>>> GetRigaOrdini()
        {
            return Ok(await _context.RigaOrdini.ToListAsync());
        }

        [HttpGet("{id}")]
        // GET: api/RigaOrdini/5
        public async Task<ActionResult<RigaOrdine>> GetRigaOrdine(int id)
        {
            var rigaOrdine = await _context.RigaOrdini.FindAsync(id);

            if (rigaOrdine == null)
            {
                return NotFound();
            }

            return Ok(rigaOrdine);
        }

        [HttpPost]
        // POST: api/RigaOrdini
        // Nota: TotaleRiga non va valorizzata qui: e' una colonna calcolata PERSISTED,
        // configurata in OnModelCreating con HasComputedColumnSql(..., stored: true).
        // EF la tratta come store-generated e la esclude dall'INSERT/UPDATE anche se
        // il body della richiesta la contiene: il valore viene sempre calcolato dal DB.
        public async Task<ActionResult<RigaOrdine>> PostRigaOrdine(RigaOrdine rigaOrdine)
        {
            var erroreFk = await ValidaForeignKeyAsync(rigaOrdine);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            _context.RigaOrdini.Add(rigaOrdine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRigaOrdine), new { id = rigaOrdine.IdRigaOrdine }, rigaOrdine);
        }

        [HttpPut("{id}")]
        // PUT: api/RigaOrdini/5
        public async Task<IActionResult> PutRigaOrdine(int id, RigaOrdine rigaOrdine)
        {
            if (id != rigaOrdine.IdRigaOrdine)
            {
                return BadRequest();
            }

            var erroreFk = await ValidaForeignKeyAsync(rigaOrdine);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            _context.Entry(rigaOrdine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RigaOrdineExists(id))
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
        // DELETE: api/RigaOrdini/5
        public async Task<IActionResult> DeleteRigaOrdine(int id)
        {
            var rigaOrdine = await _context.RigaOrdini.FindAsync(id);
            if (rigaOrdine == null)
            {
                return NotFound();
            }

            _context.RigaOrdini.Remove(rigaOrdine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string?> ValidaForeignKeyAsync(RigaOrdine rigaOrdine)
        {
            if (!await _context.Ordini.AnyAsync(o => o.IdOrdine == rigaOrdine.IdOrdine))
            {
                return "L'ordine associato non esiste.";
            }

            if (!await _context.Prodotti.AnyAsync(p => p.IdProdotto == rigaOrdine.IdProdotto))
            {
                return "Il prodotto associato non esiste.";
            }

            return null;
        }

        private bool RigaOrdineExists(int id)
        {
            return _context.RigaOrdini.Any(e => e.IdRigaOrdine == id);
        }
    }
}
