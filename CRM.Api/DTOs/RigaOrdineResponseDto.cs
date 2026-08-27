namespace CRM.Api.DTOs;

  public class RigaOrdineResponseDto
  {
    public int IdRigaOrdine { get; set; }
    public int IdProdotto { get; set; }
    public required string NomeProdotto { get; set; }
    public decimal PrezzoPattuito { get; set; }
    public decimal Sconto { get; set; }
    public int Quantita { get; set; }
    public decimal? TotaleRiga { get; set; }
  }