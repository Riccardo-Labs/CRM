using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

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
        public async Task<ActionResult<Ordine>> PostOrdine(OrdineCreateDto ordine)
        {
            var erroreFk = await ValidaForeignKeyAsync(ordine.IdAgente, ordine.IdAziendaCliente, ordine.IdContattoRiferimento);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            var nuovoOrdine = new Ordine
            {
                IdAziendaCliente = ordine.IdAziendaCliente,
                IdAgente = ordine.IdAgente,
                IdContattoRiferimento = ordine.IdContattoRiferimento,
                Stato = "Aperto",
                Note = ordine.Note
                // DataOrdine non valorizzata: resta al default CLR (non impostato),
                // EF la esclude dall'INSERT e lascia applicare il default DB GETDATE().
            };
            _context.Ordini.Add(nuovoOrdine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrdine), new { id = nuovoOrdine.IdOrdine }, nuovoOrdine);
        }

        [HttpPut("{id}")]
        // PUT: api/Ordini/5
        public async Task<IActionResult> PutOrdine(int id, OrdineUpdateDto ordine)
        {
            var ordineEsistente = await _context.Ordini.FindAsync(id);
            if (ordineEsistente == null)
            {
                return NotFound();
            }

            var erroreFk = await ValidaForeignKeyAsync(ordine.IdAgente, ordine.IdAziendaCliente, ordine.IdContattoRiferimento);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            var statiChiusi = new[] { "Vinto", "Perso", "Annullato" };
            if (statiChiusi.Contains(ordineEsistente.Stato) && ordine.Stato != ordineEsistente.Stato)
            {
                return BadRequest($"L'ordine è già chiuso (stato: {ordineEsistente.Stato}): non è possibile modificarne lo stato.");
            }

            ordineEsistente.IdAziendaCliente = ordine.IdAziendaCliente;
            ordineEsistente.IdAgente = ordine.IdAgente;
            ordineEsistente.IdContattoRiferimento = ordine.IdContattoRiferimento;
            ordineEsistente.Stato = ordine.Stato;
            ordineEsistente.Note = ordine.Note;

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

        private async Task<string?> ValidaForeignKeyAsync(int idAgente, int idAziendaCliente, int? idContattoRiferimento)
        {
            if (!await _context.Agenti.AnyAsync(a => a.IdAgente == idAgente))
            {
                return "L'agente associato non esiste.";
            }

            if (!await _context.AziendaClienti.AnyAsync(a => a.IdAziendaCliente == idAziendaCliente))
            {
                return "L'azienda cliente associata non esiste.";
            }

            if (idContattoRiferimento != null &&
                !await _context.Contatti.AnyAsync(c => c.IdContatto == idContattoRiferimento))
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