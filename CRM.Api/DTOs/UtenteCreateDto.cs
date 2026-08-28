using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class UtenteCreateDto
{
    [MaxLength(100)]
    public required string Email { get; set; }

    [MinLength(6)]
    public required string Password { get; set; }

    [MaxLength(20)]
    public required string Ruolo { get; set; }

    public int? IdAgente { get; set; }
}
