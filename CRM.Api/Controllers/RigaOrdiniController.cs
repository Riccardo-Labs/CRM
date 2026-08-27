using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

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
        // Nota: TotaleRiga non compare nel DTO: e' una colonna calcolata PERSISTED,
        // configurata in OnModelCreating con HasComputedColumnSql(..., stored: true).
        // Il valore viene sempre calcolato dal DB, il client non deve/puo' fornirlo.
        public async Task<ActionResult<RigaOrdine>> PostRigaOrdine(RigaOrdineCreateDto rigaOrdine)
        {
            var erroreFk = await ValidaForeignKeyAsync(rigaOrdine.IdOrdine, rigaOrdine.IdProdotto);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            var nuovaRigaOrdine = new RigaOrdine
            {
                IdOrdine = rigaOrdine.IdOrdine,
                IdProdotto = rigaOrdine.IdProdotto,
                Quantita = rigaOrdine.Quantita,
                PrezzoPattuito = rigaOrdine.PrezzoPattuito,
                Sconto = rigaOrdine.Sconto
            };
            _context.RigaOrdini.Add(nuovaRigaOrdine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRigaOrdine), new { id = nuovaRigaOrdine.IdRigaOrdine }, nuovaRigaOrdine);
        }

        [HttpPut("{id}")]
        // PUT: api/RigaOrdini/5
        public async Task<IActionResult> PutRigaOrdine(int id, RigaOrdineUpdateDto rigaOrdine)
        {
            var rigaOrdineEsistente = await _context.RigaOrdini.FindAsync(id);
            if (rigaOrdineEsistente == null)
            {
                return NotFound();
            }

            var erroreFk = await ValidaForeignKeyAsync(rigaOrdine.IdOrdine, rigaOrdine.IdProdotto);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            rigaOrdineEsistente.IdOrdine = rigaOrdine.IdOrdine;
            rigaOrdineEsistente.IdProdotto = rigaOrdine.IdProdotto;
            rigaOrdineEsistente.Quantita = rigaOrdine.Quantita;
            rigaOrdineEsistente.PrezzoPattuito = rigaOrdine.PrezzoPattuito;
            rigaOrdineEsistente.Sconto = rigaOrdine.Sconto;

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

        private async Task<string?> ValidaForeignKeyAsync(int idOrdine, int idProdotto)
        {
            if (!await _context.Ordini.AnyAsync(o => o.IdOrdine == idOrdine))
            {
                return "L'ordine associato non esiste.";
            }

            if (!await _context.Prodotti.AnyAsync(p => p.IdProdotto == idProdotto))
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
