namespace CRM.Api.DTOs;

public class OrdineResponseDto
{
    public int IdOrdine { get; set; }
    public int IdAziendaCliente { get; set; }
    public required string RagioneSocialeAzienda { get; set; }
    public int IdAgente { get; set; }
    public required string NomeAgente { get; set; }
    public int? IdContattoRiferimento { get; set; }
    public string? NomeContattoRiferimento { get; set; }
    public required string Stato { get; set; }
    public DateTime DataOrdine { get; set; }
    public required List<RigaOrdineResponseDto> RigaOrdini { get; set; }
}
