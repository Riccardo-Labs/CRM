using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniController : ControllerBase
    {
        private readonly CrmContext _context;

        public OrdiniController(CrmContext context)
        {
            _context = context;
        }

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

        private bool OrdineExists(int id)
        {
            return _context.Ordini.Any(e => e.IdOrdine == id);
        }
    }
}