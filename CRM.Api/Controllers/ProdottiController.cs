using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRM.Data.Models;
using CRM.Api.DTOs;

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
    public async Task<ActionResult<Prodotto>> PostProdotto(ProdottoCreateDto prodotto)
    {
        var nuovoProdotto = new Prodotto
        {
            Nome = prodotto.Nome,
            Descrizione = prodotto.Descrizione,
            Tipo = prodotto.Tipo,
            Codice = prodotto.Codice,
            PrezzoListino = prodotto.PrezzoListino,
            Attivo = true
        };
        _context.Prodotti.Add(nuovoProdotto);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Codice già in uso da un altro prodotto.");
        }

        return CreatedAtAction(nameof(GetProdotto), new { id = nuovoProdotto.IdProdotto }, nuovoProdotto);
    }

    [HttpPut("{id}")]
    // PUT: api/Prodotti/5
    public async Task<IActionResult> PutProdotto(int id, ProdottoUpdateDto prodotto)
    {
        var prodottoEsistente = await _context.Prodotti.FindAsync(id);
        if (prodottoEsistente == null)
        {
            return NotFound();
        }

        prodottoEsistente.Nome = prodotto.Nome;
        prodottoEsistente.Descrizione = prodotto.Descrizione;
        prodottoEsistente.Tipo = prodotto.Tipo;
        prodottoEsistente.Codice = prodotto.Codice;
        prodottoEsistente.PrezzoListino = prodotto.PrezzoListino;
        prodottoEsistente.Attivo = prodotto.Attivo;

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
        catch (DbUpdateException)
        {
            return Conflict("Codice già in uso da un altro prodotto.");
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
