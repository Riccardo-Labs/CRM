using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class Contatto
{
    public int IdContatto { get; set; }

    public int IdAziendaCliente { get; set; }

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string? Ruolo { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Cellulare { get; set; }

    public string? Note { get; set; }

    public bool Attivo { get; set; }

    public virtual AziendaCliente IdAziendaClienteNavigation { get; set; } = null!;

    public virtual ICollection<LogAttivita> LogAttivita { get; set; } = new List<LogAttivita>();

    public virtual ICollection<Ordine> Ordini { get; set; } = new List<Ordine>();
}
