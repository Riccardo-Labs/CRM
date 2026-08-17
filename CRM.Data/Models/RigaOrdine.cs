using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class RigaOrdine
{
    public int IdRigaOrdine { get; set; }

    public int IdOrdine { get; set; }

    public int IdProdotto { get; set; }

    public int Quantita { get; set; }

    public decimal PrezzoPattuito { get; set; }

    public decimal Sconto { get; set; }

    public decimal? TotaleRiga { get; set; }

    public virtual Ordine IdOrdineNavigation { get; set; } = null!;

    public virtual Prodotto IdProdottoNavigation { get; set; } = null!;
}
