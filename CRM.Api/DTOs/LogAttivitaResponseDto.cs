namespace CRM.Api.DTOs;

public class LogAttivitaResponseDto
{
    public int IdLogAttivita { get; set; }
    public int? IdOrdine { get; set; }
    public int? IdContatto { get; set; }
    public string? NomeContatto { get; set; }
    public int IdAgente { get; set; }
    public required string NomeAgente { get; set; }
    public DateTime DataOra { get; set; }
    public required string TipoAttivita { get; set; }
    public string? Oggetto { get; set; }
    public string? Descrizione { get; set; }
    public string? Esito { get; set; }
    public string? AllegatoUrl { get; set; }
}
