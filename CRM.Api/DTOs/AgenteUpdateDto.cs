using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class AgenteUpdateDto
{
    [MaxLength(50)]
    public required string Nome { get; set; }

    [MaxLength(50)]
    public required string Cognome { get; set; }

    [MaxLength(100)]
    public required string Email { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    public required DateOnly DataAssunzione { get; set; }
    public required bool Attivo { get; set; }
}
