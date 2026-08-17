using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class Agente
{
    public int IdAgente { get; set; }

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateOnly DataAssunzione { get; set; }

    public bool Attivo { get; set; }

    public virtual ICollection<LogAttivita> LogAttivita { get; set; } = new List<LogAttivita>();

    public virtual ICollection<Ordine> Ordini { get; set; } = new List<Ordine>();
}
