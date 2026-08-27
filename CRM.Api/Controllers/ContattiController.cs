using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

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
        public async Task<ActionResult<Contatto>> GetContatto(int id)
        {
            var contatto = await _context.Contatti.FindAsync(id);
            if (contatto == null)
            {
                return NotFound();
            }
            return Ok(contatto);
        }

        [HttpPost]
        // POST: api/Contatti
        public async Task<ActionResult<Contatto>> PostContatto(Contatto contatto)
        {
            var aziendaCliente = await _context.AziendaClienti.FindAsync(contatto.IdAziendaCliente);
            if (aziendaCliente == null)
            {
                return BadRequest("L'azienda cliente associata non esiste.");
            }

            _context.Contatti.Add(contatto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContatto), new { id = contatto.IdContatto }, contatto);
        }

        [HttpPut("{id}")]
        // PUT: api/Contatti/5
        public async Task<IActionResult> PutContatto(int id, Contatto contatto)
        {
            if (id != contatto.IdContatto)
            {
                return BadRequest();
            }

            var aziendaCliente = await _context.AziendaClienti.FindAsync(contatto.IdAziendaCliente);
            if (aziendaCliente == null)
            {
                return BadRequest("L'azienda cliente associata non esiste.");
            }

            _context.Entry(contatto).State = EntityState.Modified;

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