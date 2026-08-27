namespace CRM.Api.DTOs;
public class AziendaClienteResponseDto
{
    public int IdAziendaCliente { get; set; }

    public required string RagioneSociale { get; set; }

    public required string PartitaIva { get; set; }

    public string? CodiceFiscale { get; set; }

    public string? Indirizzo { get; set; }

    public string? Citta { get; set; }

    public string? Cap { get; set; }

    public string? Provincia { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? SitoWeb { get; set; }

    public string? Note { get; set; }

    public required List<ContattoResponseDto> Contatti { get; set; }
}