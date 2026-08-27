using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class ProdottoCreateDto
{
    [MaxLength(100)]
    public required string Nome { get; set; }

    [MaxLength(500)]
    public string? Descrizione { get; set; }

    [MaxLength(20)]
    public required string Tipo { get; set; }

    [MaxLength(30)]
    public required string Codice { get; set; }

    public required decimal PrezzoListino { get; set; }

    // Attivo viene forzato lato server, quindi non è incluso nel DTO
}
