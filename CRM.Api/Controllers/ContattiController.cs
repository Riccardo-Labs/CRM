using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContattiController(CrmContext context) : ControllerBase
    {
        private readonly CrmContext _context = context;

        [HttpGet]
        // GET: api/Contatti
        public async Task<ActionResult<IEnumerable<Contatto>>> GetContatti()
        {
            var contatti = await _context.Contatti.Where(c => c.Attivo).ToListAsync();
            return Ok(contatti);
        }

        [HttpGet("{id}")]
        // GET: api/Contatti/5
        public async Task<ActionResult<ContattoResponseDto>> GetContatto(int id)
        {
            var contatto = await _context.Contatti
                .Include(c => c.IdAziendaClienteNavigation)
                .FirstOrDefaultAsync(c => c.IdContatto == id);

            if (contatto == null)
            {
                return NotFound();
            }

            var risposta = new ContattoResponseDto
            {
                IdContatto = contatto.IdContatto,
                IdAziendaCliente = contatto.IdAziendaCliente,
                RagioneSocialeAzienda = contatto.IdAziendaClienteNavigation.RagioneSociale,
                Nome = contatto.Nome,
                Cognome = contatto.Cognome,
                Ruolo = contatto.Ruolo,
                Email = contatto.Email,
                Telefono = contatto.Telefono,
                Cellulare = contatto.Cellulare
            };

            return Ok(risposta);
        }

        [HttpPost]
        // POST: api/Contatti
        public async Task<ActionResult<Contatto>> PostContatto(ContattoCreateDto contatto)
        {
            if (!await _context.AziendaClienti.AnyAsync(a => a.IdAziendaCliente == contatto.IdAziendaCliente))
            {
                return BadRequest("L'azienda cliente associata non esiste.");
            }

            var nuovoContatto = new Contatto
            {
                IdAziendaCliente = contatto.IdAziendaCliente,
                Nome = contatto.Nome,
                Cognome = contatto.Cognome,
                Ruolo = contatto.Ruolo,
                Email = contatto.Email,
                Telefono = contatto.Telefono,
                Cellulare = contatto.Cellulare,
                Note = contatto.Note,
                Attivo = true
            };
            _context.Contatti.Add(nuovoContatto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContatto), new { id = nuovoContatto.IdContatto }, nuovoContatto);
        }

        [HttpPut("{id}")]
        // PUT: api/Contatti/5
        public async Task<IActionResult> PutContatto(int id, ContattoUpdateDto contatto)
        {
            var contattoEsistente = await _context.Contatti.FindAsync(id);
            if (contattoEsistente == null)
            {
                return NotFound();
            }

            if (!await _context.AziendaClienti.AnyAsync(a => a.IdAziendaCliente == contatto.IdAziendaCliente))
            {
                return BadRequest("L'azienda cliente associata non esiste.");
            }

            contattoEsistente.IdAziendaCliente = contatto.IdAziendaCliente;
            contattoEsistente.Nome = contatto.Nome;
            contattoEsistente.Cognome = contatto.Cognome;
            contattoEsistente.Ruolo = contatto.Ruolo;
            contattoEsistente.Email = contatto.Email;
            contattoEsistente.Telefono = contatto.Telefono;
            contattoEsistente.Cellulare = contatto.Cellulare;
            contattoEsistente.Note = contatto.Note;
            contattoEsistente.Attivo = contatto.Attivo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContattoExists(id))
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
        // DELETE: api/Contatti/5
        public async Task<IActionResult> DeleteContatto(int id)
        {
            var contatto = await _context.Contatti.FindAsync(id);
            if (contatto == null)
            {
                return NotFound();
            }

            contatto.Attivo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContattoExists(int id)
        {
            return _context.Contatti.Any(e => e.IdContatto == id);
        }
    }
}