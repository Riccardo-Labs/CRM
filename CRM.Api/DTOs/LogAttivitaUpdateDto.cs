using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class LogAttivitaUpdateDto
{
    public int? IdOrdine { get; set; }
    public int? IdContatto { get; set; }
    public required int IdAgente { get; set; }

    [MaxLength(20)]
    public required string TipoAttivita { get; set; }

    [MaxLength(150)]
    public string? Oggetto { get; set; }

    public string? Descrizione { get; set; }

    [MaxLength(100)]
    public string? Esito { get; set; }

    [MaxLength(300)]
    public string? AllegatoUrl { get; set; }

    // DataOra esclusa anche in update: il momento in cui l'attivita' e' avvenuta
    // non e' pensato per essere modificabile a posteriori.
}
