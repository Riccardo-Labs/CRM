using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AziendaClientiController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/AziendaClienti
        public async Task<ActionResult<IEnumerable<AziendaCliente>>> GetAziendeClienti()
        {
            var aziendeClienti = await _context.AziendaClienti.Where(a => a.Attivo).ToListAsync();
            return Ok(aziendeClienti);
        }

        [HttpGet("{id}")]
        // GET: api/AziendaClienti/5
        public async Task<ActionResult<AziendaClienteResponseDto>> GetAziendaCliente(int id)
        {
            var aziendaCliente = await _context.AziendaClienti
                .Include(a => a.Contatti)
                .FirstOrDefaultAsync(a => a.IdAziendaCliente == id);
            
            if (aziendaCliente == null)
            {
                return NotFound();
            }

            var risposta = new AziendaClienteResponseDto
            {
                IdAziendaCliente = aziendaCliente.IdAziendaCliente,
                RagioneSociale = aziendaCliente.RagioneSociale,
                PartitaIva = aziendaCliente.PartitaIva,
                CodiceFiscale = aziendaCliente.CodiceFiscale,
                Indirizzo = aziendaCliente.Indirizzo,
                Citta = aziendaCliente.Citta,
                Cap = aziendaCliente.Cap,
                Provincia = aziendaCliente.Provincia,
                Email = aziendaCliente.Email,
                Telefono = aziendaCliente.Telefono,
                SitoWeb = aziendaCliente.SitoWeb,
                Note = aziendaCliente.Note,
                Contatti = aziendaCliente.Contatti.Where(c => c.Attivo).Select(c => new ContattoResponseDto
                {
                    IdContatto = c.IdContatto,
                    Nome = c.Nome,
                    Cognome = c.Cognome,
                    Ruolo = c.Ruolo,
                    Email = c.Email,
                    Telefono = c.Telefono,
                    Cellulare = c.Cellulare
                }).ToList()
            };

            return Ok(risposta);
        }

        [HttpPost]
        // POST: api/AziendaClienti
        public async Task<ActionResult<AziendaCliente>> PostAziendaCliente(AziendaClienteCreateDto aziendaCliente)
        {
            var nuovaAziendaCliente = new AziendaCliente
            {
                RagioneSociale = aziendaCliente.RagioneSociale,
                PartitaIva = aziendaCliente.PartitaIva,
                CodiceFiscale = aziendaCliente.CodiceFiscale,
                Indirizzo = aziendaCliente.Indirizzo,
                Citta = aziendaCliente.Citta,
                Cap = aziendaCliente.Cap,
                Provincia = aziendaCliente.Provincia,
                Email = aziendaCliente.Email,
                Telefono = aziendaCliente.Telefono,
                SitoWeb = aziendaCliente.SitoWeb,
                Note = aziendaCliente.Note,
                Attivo = true
            };
            _context.AziendaClienti.Add(nuovaAziendaCliente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Partita IVA già in uso da un'altra azienda cliente.");
            }

            return CreatedAtAction(nameof(GetAziendaCliente), new { id = nuovaAziendaCliente.IdAziendaCliente }, nuovaAziendaCliente);
        }

        [HttpPut("{id}")]
        // PUT: api/AziendaClienti/5
        public async Task<IActionResult> PutAziendaCliente(int id, AziendaClienteUpdateDto aziendaCliente)
        {
            var aziendaClienteEsistente = await _context.AziendaClienti.FindAsync(id);
            if (aziendaClienteEsistente == null)
            {
                return NotFound();
            }

            aziendaClienteEsistente.RagioneSociale = aziendaCliente.RagioneSociale;
            aziendaClienteEsistente.PartitaIva = aziendaCliente.PartitaIva;
            aziendaClienteEsistente.CodiceFiscale = aziendaCliente.CodiceFiscale;
            aziendaClienteEsistente.Indirizzo = aziendaCliente.Indirizzo;
            aziendaClienteEsistente.Citta = aziendaCliente.Citta;
            aziendaClienteEsistente.Cap = aziendaCliente.Cap;
            aziendaClienteEsistente.Provincia = aziendaCliente.Provincia;
            aziendaClienteEsistente.Email = aziendaCliente.Email;
            aziendaClienteEsistente.Telefono = aziendaCliente.Telefono;
            aziendaClienteEsistente.SitoWeb = aziendaCliente.SitoWeb;
            aziendaClienteEsistente.Note = aziendaCliente.Note;
            aziendaClienteEsistente.Attivo = aziendaCliente.Attivo;

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
            catch (DbUpdateException)
            {
                return Conflict("Partita IVA già in uso da un'altra azienda cliente.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        // DELETE: api/AziendaClienti/5
        public async Task<IActionResult> DeleteAziendaCliente(int id)
        {
            var aziendaCliente = await _context.AziendaClienti.FindAsync(id);
            if (aziendaCliente == null)
            {
                return NotFound();
            }

            aziendaCliente.Attivo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AziendaClienteExists(int id)
        {
            return _context.AziendaClienti.Any(e => e.IdAziendaCliente == id);
        }
    }
}