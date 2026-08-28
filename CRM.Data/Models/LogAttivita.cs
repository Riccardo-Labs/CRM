using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class LogAttivita
{
    public int IdLogAttivita { get; set; }

    public int? IdOrdine { get; set; }

    public int? IdContatto { get; set; }

    public int IdAgente { get; set; }

    public DateTime DataOra { get; set; }

    public string TipoAttivita { get; set; } = null!;

    public string? Oggetto { get; set; }

    public string? Descrizione { get; set; }

    public string? Esito { get; set; }

    public string? AllegatoUrl { get; set; }

    public virtual Agente IdAgenteNavigation { get; set; } = null!;

    public virtual Contatto? IdContattoNavigation { get; set; }

    public virtual Ordine? IdOrdineNavigation { get; set; }
}
