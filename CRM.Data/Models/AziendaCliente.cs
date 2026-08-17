using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class AziendaCliente
{
    public int IdAziendaCliente { get; set; }

    public string RagioneSociale { get; set; } = null!;

    public string PartitaIva { get; set; } = null!;

    public string? CodiceFiscale { get; set; }

    public string? Indirizzo { get; set; }

    public string? Citta { get; set; }

    public string? Cap { get; set; }

    public string? Provincia { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? SitoWeb { get; set; }

    public string? Note { get; set; }

    public bool Attivo { get; set; }

    public virtual ICollection<Contatto> Contatti { get; set; } = new List<Contatto>();

    public virtual ICollection<Ordine> Ordini { get; set; } = new List<Ordine>();
}
