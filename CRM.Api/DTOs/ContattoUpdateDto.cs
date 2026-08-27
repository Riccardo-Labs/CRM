using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class ContattoUpdateDto
{
    public required int IdAziendaCliente { get; set; }

    [MaxLength(50)]
    public required string Nome { get; set; }

    [MaxLength(50)]
    public required string Cognome { get; set; }

    [MaxLength(50)]
    public string? Ruolo { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(20)]
    public string? Cellulare { get; set; }

    public string? Note { get; set; }
    public required bool Attivo { get; set; }
}
