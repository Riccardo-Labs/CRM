namespace CRM.Api.DTOs;

public class UtenteResponseDto
{
    public int IdUtente { get; set; }
    public required string Email { get; set; }
    public required string Ruolo { get; set; }
    public int? IdAgente { get; set; }
    public bool Attivo { get; set; }
    public DateTime DataCreazione { get; set; }
}
