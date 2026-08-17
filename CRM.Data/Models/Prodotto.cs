using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class Prodotto
{
    public int IdProdotto { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descrizione { get; set; }

    public string Tipo { get; set; } = null!;

    public string Codice { get; set; } = null!;

    public decimal PrezzoListino { get; set; }

    public bool Attivo { get; set; }

    public virtual ICollection<RigaOrdine> RigaOrdini { get; set; } = new List<RigaOrdine>();
}
