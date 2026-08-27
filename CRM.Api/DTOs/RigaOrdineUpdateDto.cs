using System.ComponentModel.DataAnnotations;

namespace CRM.Api.DTOs;

public class RigaOrdineUpdateDto
{
    public required int IdOrdine { get; set; }
    public required int IdProdotto { get; set; }

    [Range(1, int.MaxValue)]
    public required int Quantita { get; set; }

    public required decimal PrezzoPattuito { get; set; }
    public decimal Sconto { get; set; }
}
