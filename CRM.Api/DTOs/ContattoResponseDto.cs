namespace CRM.Api.DTOs;

public class ContattoResponseDto
{
  public int IdContatto { get; set; }
  public int IdAziendaCliente { get; set; }
  public required string RagioneSocialeAzienda { get; set; }
  public required string Nome { get; set; }
  public required string Cognome { get; set; }
  public string? Ruolo { get; set; }
  public string? Email { get; set; }
  public string? Telefono { get; set; }
  public string? Cellulare { get; set; }
}
