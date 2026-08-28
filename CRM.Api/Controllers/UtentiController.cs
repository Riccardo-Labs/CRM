using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UtentiController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;
        private static readonly string[] RuoliValidi = { "Admin", "Agente" };

        [HttpGet]
        // GET: api/Utenti
        public async Task<ActionResult<IEnumerable<UtenteResponseDto>>> GetUtenti()
        {
            var utenti = await _context.Utenti
                .Where(u => u.Attivo)
                .Select(u => new UtenteResponseDto
                {
                    IdUtente = u.IdUtente,
                    Email = u.Email,
                    Ruolo = u.Ruolo,
                    IdAgente = u.IdAgente,
                    Attivo = u.Attivo,
                    DataCreazione = u.DataCreazione
                })
                .ToListAsync();

            return Ok(utenti);
        }

        [HttpGet("{id}")]
        // GET: api/Utenti/5
        public async Task<ActionResult<UtenteResponseDto>> GetUtente(int id)
        {
            var utente = await _context.Utenti.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }

            var risposta = new UtenteResponseDto
            {
                IdUtente = utente.IdUtente,
                Email = utente.Email,
                Ruolo = utente.Ruolo,
                IdAgente = utente.IdAgente,
                Attivo = utente.Attivo,
                DataCreazione = utente.DataCreazione
            };

            return Ok(risposta);
        }

        [HttpPost]
        // POST: api/Utenti
        public async Task<ActionResult<UtenteResponseDto>> PostUtente(UtenteCreateDto utente)
        {
            if (!RuoliValidi.Contains(utente.Ruolo))
            {
                return BadRequest("Ruolo non valido: deve essere 'Admin' o 'Agente'.");
            }

            var erroreRuolo = ValidaCoerenzaRuoloAgente(utente.Ruolo, utente.IdAgente);
            if (erroreRuolo != null)
            {
                return BadRequest(erroreRuolo);
            }

            if (utente.IdAgente != null &&
                !await _context.Agenti.AnyAsync(a => a.IdAgente == utente.IdAgente))
            {
                return BadRequest("L'agente associato non esiste.");
            }

            var nuovoUtente = new Utente
            {
                Email = utente.Email,
                PasswordHash = string.Empty,
                Ruolo = utente.Ruolo,
                IdAgente = utente.IdAgente,
                Attivo = true
            };

            var hasher = new PasswordHasher<Utente>();
            nuovoUtente.PasswordHash = hasher.HashPassword(nuovoUtente, utente.Password);

            _context.Utenti.Add(nuovoUtente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Email già in uso o agente già associato a un altro utente.");
            }

            var risposta = new UtenteResponseDto
            {
                IdUtente = nuovoUtente.IdUtente,
                Email = nuovoUtente.Email,
                Ruolo = nuovoUtente.Ruolo,
                IdAgente = nuovoUtente.IdAgente,
                Attivo = nuovoUtente.Attivo,
                DataCreazione = nuovoUtente.DataCreazione
            };

            return CreatedAtAction(nameof(GetUtente), new { id = nuovoUtente.IdUtente }, risposta);
        }

        [HttpPut("{id}")]
        // PUT: api/Utenti/5
        public async Task<IActionResult> PutUtente(int id, UtenteUpdateDto utente)
        {
            var utenteEsistente = await _context.Utenti.FindAsync(id);
            if (utenteEsistente == null)
            {
                return NotFound();
            }

            if (!RuoliValidi.Contains(utente.Ruolo))
            {
                return BadRequest("Ruolo non valido: deve essere 'Admin' o 'Agente'.");
            }

            var erroreRuolo = ValidaCoerenzaRuoloAgente(utente.Ruolo, utente.IdAgente);
            if (erroreRuolo != null)
            {
                return BadRequest(erroreRuolo);
            }

            if (utente.IdAgente != null &&
                !await _context.Agenti.AnyAsync(a => a.IdAgente == utente.IdAgente))
            {
                return BadRequest("L'agente associato non esiste.");
            }

            utenteEsistente.Email = utente.Email;
            utenteEsistente.Ruolo = utente.Ruolo;
            utenteEsistente.IdAgente = utente.IdAgente;
            utenteEsistente.Attivo = utente.Attivo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtenteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException)
            {
                return Conflict("Email già in uso o agente già associato a un altro utente.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        // DELETE: api/Utenti/5
        public async Task<IActionResult> DeleteUtente(int id)
        {
            var utente = await _context.Utenti.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }

            utente.Attivo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static string? ValidaCoerenzaRuoloAgente(string ruolo, int? idAgente)
        {
            if (ruolo == "Agente" && idAgente == null)
            {
                return "Un utente con ruolo Agente deve essere collegato a un agente.";
            }

            if (ruolo == "Admin" && idAgente != null)
            {
                return "Un utente con ruolo Admin non può essere collegato a un agente.";
            }

            return null;
        }

        private bool UtenteExists(int id)
        {
            return _context.Utenti.Any(e => e.IdUtente == id);
        }
    }
}
