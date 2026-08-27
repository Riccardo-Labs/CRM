using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;

namespace CRM.Api.Controllers
{
    
[Route("api/[controller]")]
[ApiController]
public class ProdottiController(CrmContext context) : ControllerBase
{
    private readonly CrmContext _context = context;

    [HttpGet]
    // GET: api/Prodotti
    public async Task<ActionResult<IEnumerable<Prodotto>>> GetProdotti()
    {
        return await _context.Prodotti.Where(p => p.Attivo).ToListAsync();
    }

    [HttpGet("{id}")]
    // GET: api/Prodotti/5
    public async Task<ActionResult<Prodotto>> GetProdotto(int id)
    {
        var prodotto = await _context.Prodotti.FindAsync(id);

        if (prodotto == null)
        {
            return NotFound();
        }

        return prodotto;
    }

    [HttpPost]
    // POST: api/Prodotti
    public async Task<ActionResult<Prodotto>> PostProdotto(Prodotto prodotto)
    {
        _context.Prodotti.Add(prodotto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProdotto), new { id = prodotto.IdProdotto }, prodotto);
    }

    [HttpPut("{id}")]
    // PUT: api/Prodotti/5
    public async Task<IActionResult> PutProdotto(int id, Prodotto prodotto)
    {
        if (id != prodotto.IdProdotto)
        {
            return BadRequest();
        }

        _context.Entry(prodotto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdottoExists(id))
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

    private bool ProdottoExists(int id)
    {
        return _context.Prodotti.Any(e => e.IdProdotto == id);
    }
    
    [HttpDelete("{id}")]
    // DELETE: api/Prodotti/5
    public async Task<IActionResult> DeleteProdotto(int id)
    {
        var prodotto = await _context.Prodotti.FindAsync(id);
        if (prodotto == null)
        {
            return NotFound();
        }

        prodotto.Attivo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
}
