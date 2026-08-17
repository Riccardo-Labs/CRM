using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class Ordine
{
    public int IdOrdine { get; set; }

    public int IdAziendaCliente { get; set; }

    public int IdAgente { get; set; }

    public int? IdContattoRiferimento { get; set; }

    public DateTime DataOrdine { get; set; }

    public string Stato { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Agente IdAgenteNavigation { get; set; } = null!;

    public virtual AziendaCliente IdAziendaClienteNavigation { get; set; } = null!;

    public virtual Contatto? IdContattoRiferimentoNavigation { get; set; }

    public virtual ICollection<LogAttivita> LogAttivita { get; set; } = new List<LogAttivita>();

    public virtual ICollection<RigaOrdine> RigaOrdini { get; set; } = new List<RigaOrdine>();
}
