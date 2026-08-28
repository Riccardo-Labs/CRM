using System;
using System.Collections.Generic;

namespace CRM.Data.Models;

public partial class Utente
{
    public int IdUtente { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Ruolo { get; set; } = null!;

    public int? IdAgente { get; set; }

    public bool Attivo { get; set; }

    public DateTime DataCreazione { get; set; }

    public virtual Agente? IdAgenteNavigation { get; set; }
}
