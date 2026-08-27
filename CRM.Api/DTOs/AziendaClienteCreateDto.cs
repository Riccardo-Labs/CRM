using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class AziendaClienteCreateDto
{
    [MaxLength(150)]
    public required string RagioneSociale { get; set; }

    [MaxLength(20)]
    public required string PartitaIva { get; set; }

    [MaxLength(20)]
    public string? CodiceFiscale { get; set; }

    [MaxLength(150)]
    public string? Indirizzo { get; set; }

    [MaxLength(50)]
    public string? Citta { get; set; }

    [MaxLength(10)]
    public string? Cap { get; set; }

    [MaxLength(2)]
    public string? Provincia { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(150)]
    public string? SitoWeb { get; set; }

    public string? Note { get; set; }

    // Attivo forzato lato server a true, come per Agente/Prodotto
}
