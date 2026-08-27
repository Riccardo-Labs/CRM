using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

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
        public async Task<ActionResult<LogAttivita>> PostLogAttivita(LogAttivitaCreateDto logAttivita)
        {
            var erroreFk = await ValidaForeignKeyAsync(logAttivita.IdAgente, logAttivita.IdOrdine, logAttivita.IdContatto);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            var nuovoLogAttivita = new LogAttivita
            {
                IdOrdine = logAttivita.IdOrdine,
                IdContatto = logAttivita.IdContatto,
                IdAgente = logAttivita.IdAgente,
                TipoAttivita = logAttivita.TipoAttivita,
                Oggetto = logAttivita.Oggetto,
                Descrizione = logAttivita.Descrizione,
                Esito = logAttivita.Esito,
                AllegatoUrl = logAttivita.AllegatoUrl
                // DataOra non valorizzata: EF la esclude dall'INSERT, si applica il default DB GETDATE().
            };
            _context.LogAttivita.Add(nuovoLogAttivita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogAttivitaById), new { id = nuovoLogAttivita.IdLogAttivita }, nuovoLogAttivita);
        }

        [HttpPut("{id}")]
        // PUT: api/LogAttivita/5
        public async Task<IActionResult> PutLogAttivita(int id, LogAttivitaUpdateDto logAttivita)
        {
            var logAttivitaEsistente = await _context.LogAttivita.FindAsync(id);
            if (logAttivitaEsistente == null)
            {
                return NotFound();
            }

            var erroreFk = await ValidaForeignKeyAsync(logAttivita.IdAgente, logAttivita.IdOrdine, logAttivita.IdContatto);
            if (erroreFk != null)
            {
                return BadRequest(erroreFk);
            }

            logAttivitaEsistente.IdOrdine = logAttivita.IdOrdine;
            logAttivitaEsistente.IdContatto = logAttivita.IdContatto;
            logAttivitaEsistente.IdAgente = logAttivita.IdAgente;
            logAttivitaEsistente.TipoAttivita = logAttivita.TipoAttivita;
            logAttivitaEsistente.Oggetto = logAttivita.Oggetto;
            logAttivitaEsistente.Descrizione = logAttivita.Descrizione;
            logAttivitaEsistente.Esito = logAttivita.Esito;
            logAttivitaEsistente.AllegatoUrl = logAttivita.AllegatoUrl;

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
        private async Task<string?> ValidaForeignKeyAsync(int idAgente, int? idOrdine, int? idContatto)
        {
            if (!await _context.Agenti.AnyAsync(a => a.IdAgente == idAgente))
            {
                return "L'agente associato non esiste.";
            }

            if (idOrdine != null &&
                !await _context.Ordini.AnyAsync(o => o.IdOrdine == idOrdine))
            {
                return "L'ordine associato non esiste.";
            }

            if (idContatto != null &&
                !await _context.Contatti.AnyAsync(c => c.IdContatto == idContatto))
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
