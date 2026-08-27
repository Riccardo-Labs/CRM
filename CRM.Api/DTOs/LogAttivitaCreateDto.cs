using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class LogAttivitaCreateDto
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

    // DataOra forzata lato server (default DB GETDATE()): il client non la fornisce.
}
