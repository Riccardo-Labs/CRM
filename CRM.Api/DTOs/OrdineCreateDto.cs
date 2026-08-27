namespace CRM.Api.DTOs;

public class OrdineCreateDto
{
    public required int IdAziendaCliente { get; set; }
    public required int IdAgente { get; set; }
    public int? IdContattoRiferimento { get; set; }
    public string? Note { get; set; }

    // Stato forzato lato server a "Aperto": un ordine nuovo parte sempre cosi'.
    // DataOrdine forzata lato server (default DB GETDATE()): il client non la fornisce.
}
