using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class UtenteUpdateDto
{
    [MaxLength(100)]
    public required string Email { get; set; }

    [MaxLength(20)]
    public required string Ruolo { get; set; }

    public int? IdAgente { get; set; }

    public bool Attivo { get; set; }
}
