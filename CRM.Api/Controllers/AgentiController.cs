using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

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
            var agenti = await _context.Agenti.Where(a => a.Attivo).ToListAsync();
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
        public async Task<ActionResult<Agente>> PostAgente(AgenteCreateDto agente)
        {
            var newAgente = new Agente
            {
                Nome = agente.Nome,
                Cognome = agente.Cognome,
                Email = agente.Email,
                Telefono = agente.Telefono,
                DataAssunzione = agente.DataAssunzione,
                Attivo = true
            };
            _context.Agenti.Add(newAgente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict("Email già in uso da un altro agente.");
            }

            return CreatedAtAction(nameof(GetAgente), new { id = newAgente.IdAgente }, newAgente);
        }

        [HttpPut("{id}")]
        // PUT: api/Agenti/5
        public async Task<IActionResult> PutAgente(int id, AgenteUpdateDto agente)
        {
            var agenteEsistente = await _context.Agenti.FindAsync(id);
            if (agenteEsistente == null)
            {
                return NotFound();
            }

            agenteEsistente.Nome = agente.Nome;
            agenteEsistente.Cognome = agente.Cognome;
            agenteEsistente.Email = agente.Email;
            agenteEsistente.Telefono = agente.Telefono;
            agenteEsistente.DataAssunzione = agente.DataAssunzione;
            agenteEsistente.Attivo = agente.Attivo;

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
            catch (DbUpdateException)
            {
                return Conflict("Email già in uso da un altro agente.");
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
            agente.Attivo = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgenteExists(int id)
        {
            return _context.Agenti.Any(e => e.IdAgente == id);
        }
    }
}