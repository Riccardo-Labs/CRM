namespace CRM.Api.DTOs;

public class OrdineUpdateDto
{
    public required int IdAziendaCliente { get; set; }
    public required int IdAgente { get; set; }
    public int? IdContattoRiferimento { get; set; }
    public required string Stato { get; set; }
    public string? Note { get; set; }

    // DataOrdine resta esclusa anche in update: la data di creazione dell'ordine
    // non e' pensata per essere modificabile a posteriori.
}
