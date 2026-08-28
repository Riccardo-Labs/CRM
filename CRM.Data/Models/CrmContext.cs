using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Models;

public partial class CrmContext : DbContext
{
    public CrmContext(DbContextOptions<CrmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agente> Agenti { get; set; }

    public virtual DbSet<AziendaCliente> AziendaClienti { get; set; }

    public virtual DbSet<Contatto> Contatti { get; set; }

    public virtual DbSet<LogAttivita> LogAttivita { get; set; }

    public virtual DbSet<Ordine> Ordini { get; set; }

    public virtual DbSet<Prodotto> Prodotti { get; set; }

    public virtual DbSet<RigaOrdine> RigaOrdini { get; set; }

    public virtual DbSet<Utente> Utenti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agente>(entity =>
        {
            entity.HasKey(e => e.IdAgente).HasName("PK__Agente__178FE9935D19E51E");

            entity.ToTable("Agente");

            entity.HasIndex(e => e.Email, "UQ__Agente__AB6E6164B4C29278").IsUnique();

            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.Attivo)
                .HasDefaultValue(true)
                .HasColumnName("attivo");
            entity.Property(e => e.Cognome)
                .HasMaxLength(50)
                .HasColumnName("cognome");
            entity.Property(e => e.DataAssunzione).HasColumnName("data_assunzione");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<AziendaCliente>(entity =>
        {
            entity.HasKey(e => e.IdAziendaCliente).HasName("PK__AziendaC__E4272341D67119DF");

            entity.ToTable("AziendaCliente");

            entity.HasIndex(e => e.PartitaIva, "UQ__AziendaC__026FB849FC9DD341").IsUnique();

            entity.Property(e => e.IdAziendaCliente).HasColumnName("id_azienda_cliente");
            entity.Property(e => e.Attivo)
                .HasDefaultValue(true)
                .HasColumnName("attivo");
            entity.Property(e => e.Cap)
                .HasMaxLength(10)
                .HasColumnName("cap");
            entity.Property(e => e.Citta)
                .HasMaxLength(50)
                .HasColumnName("citta");
            entity.Property(e => e.CodiceFiscale)
                .HasMaxLength(20)
                .HasColumnName("codice_fiscale");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Indirizzo)
                .HasMaxLength(150)
                .HasColumnName("indirizzo");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.PartitaIva)
                .HasMaxLength(20)
                .HasColumnName("partita_iva");
            entity.Property(e => e.Provincia)
                .HasMaxLength(2)
                .HasColumnName("provincia");
            entity.Property(e => e.RagioneSociale)
                .HasMaxLength(150)
                .HasColumnName("ragione_sociale");
            entity.Property(e => e.SitoWeb)
                .HasMaxLength(150)
                .HasColumnName("sito_web");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Contatto>(entity =>
        {
            entity.HasKey(e => e.IdContatto).HasName("PK__Contatto__0479E590CE541A75");

            entity.ToTable("Contatto");

            entity.Property(e => e.IdContatto).HasColumnName("id_contatto");
            entity.Property(e => e.Attivo)
                .HasDefaultValue(true)
                .HasColumnName("attivo");
            entity.Property(e => e.Cellulare)
                .HasMaxLength(20)
                .HasColumnName("cellulare");
            entity.Property(e => e.Cognome)
                .HasMaxLength(50)
                .HasColumnName("cognome");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdAziendaCliente).HasColumnName("id_azienda_cliente");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Ruolo)
                .HasMaxLength(50)
                .HasColumnName("ruolo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdAziendaClienteNavigation).WithMany(p => p.Contatti)
                .HasForeignKey(d => d.IdAziendaCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contatto_AziendaCliente");
        });

        modelBuilder.Entity<LogAttivita>(entity =>
        {
            entity.HasKey(e => e.IdLogAttivita).HasName("PK__LogAttiv__15B313D76DB20EBF");

            entity.Property(e => e.IdLogAttivita).HasColumnName("id_log_attivita");
            entity.Property(e => e.AllegatoUrl)
                .HasMaxLength(300)
                .HasColumnName("allegato_url");
            entity.Property(e => e.DataOra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("data_ora");
            entity.Property(e => e.Descrizione).HasColumnName("descrizione");
            entity.Property(e => e.Esito)
                .HasMaxLength(100)
                .HasColumnName("esito");
            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.IdContatto).HasColumnName("id_contatto");
            entity.Property(e => e.IdOrdine).HasColumnName("id_ordine");
            entity.Property(e => e.Oggetto)
                .HasMaxLength(150)
                .HasColumnName("oggetto");
            entity.Property(e => e.TipoAttivita)
                .HasMaxLength(20)
                .HasColumnName("tipo_attivita");

            entity.HasOne(d => d.IdAgenteNavigation).WithMany(p => p.LogAttivita)
                .HasForeignKey(d => d.IdAgente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogAttivita_Agente");

            entity.HasOne(d => d.IdContattoNavigation).WithMany(p => p.LogAttivita)
                .HasForeignKey(d => d.IdContatto)
                .HasConstraintName("FK_LogAttivita_Contatto");

            entity.HasOne(d => d.IdOrdineNavigation).WithMany(p => p.LogAttivita)
                .HasForeignKey(d => d.IdOrdine)
                .HasConstraintName("FK_LogAttivita_Ordine");
        });

        modelBuilder.Entity<Ordine>(entity =>
        {
            entity.HasKey(e => e.IdOrdine).HasName("PK__Ordine__1D19D4442716F6C6");

            entity.ToTable("Ordine");

            entity.Property(e => e.IdOrdine).HasColumnName("id_ordine");
            entity.Property(e => e.DataOrdine)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("data_ordine");
            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.IdAziendaCliente).HasColumnName("id_azienda_cliente");
            entity.Property(e => e.IdContattoRiferimento).HasColumnName("id_contatto_riferimento");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Stato)
                .HasMaxLength(20)
                .HasColumnName("stato");

            entity.HasOne(d => d.IdAgenteNavigation).WithMany(p => p.Ordini)
                .HasForeignKey(d => d.IdAgente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordine_Agente");

            entity.HasOne(d => d.IdAziendaClienteNavigation).WithMany(p => p.Ordini)
                .HasForeignKey(d => d.IdAziendaCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ordine_AziendaCliente");

            entity.HasOne(d => d.IdContattoRiferimentoNavigation).WithMany(p => p.Ordini)
                .HasForeignKey(d => d.IdContattoRiferimento)
                .HasConstraintName("FK_Ordine_Contatto");
        });

        modelBuilder.Entity<Prodotto>(entity =>
        {
            entity.HasKey(e => e.IdProdotto).HasName("PK__Prodotto__EC110BE91847E957");

            entity.ToTable("Prodotto");

            entity.HasIndex(e => e.Codice, "UQ__Prodotto__40F9C18B3A74E1AD").IsUnique();

            entity.Property(e => e.IdProdotto).HasColumnName("id_prodotto");
            entity.Property(e => e.Attivo)
                .HasDefaultValue(true)
                .HasColumnName("attivo");
            entity.Property(e => e.Codice)
                .HasMaxLength(30)
                .HasColumnName("codice");
            entity.Property(e => e.Descrizione)
                .HasMaxLength(500)
                .HasColumnName("descrizione");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.PrezzoListino)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prezzo_listino");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<RigaOrdine>(entity =>
        {
            entity.HasKey(e => e.IdRigaOrdine).HasName("PK__RigaOrdi__1DFF6BF8834E4379");

            entity.ToTable("RigaOrdine");

            entity.Property(e => e.IdRigaOrdine).HasColumnName("id_riga_ordine");
            entity.Property(e => e.IdOrdine).HasColumnName("id_ordine");
            entity.Property(e => e.IdProdotto).HasColumnName("id_prodotto");
            entity.Property(e => e.PrezzoPattuito)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prezzo_pattuito");
            entity.Property(e => e.Quantita).HasColumnName("quantita");
            entity.Property(e => e.Sconto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sconto");
            entity.Property(e => e.TotaleRiga)
                .HasComputedColumnSql("([quantita]*[prezzo_pattuito]-[sconto])", true)
                .HasColumnType("decimal(22, 2)")
                .HasColumnName("totale_riga");

            entity.HasOne(d => d.IdOrdineNavigation).WithMany(p => p.RigaOrdini)
                .HasForeignKey(d => d.IdOrdine)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RigaOrdine_Ordine");

            entity.HasOne(d => d.IdProdottoNavigation).WithMany(p => p.RigaOrdini)
                .HasForeignKey(d => d.IdProdotto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RigaOrdine_Prodotto");
        });

        modelBuilder.Entity<Utente>(entity =>
        {
            entity.HasKey(e => e.IdUtente).HasName("PK__Utente__43BCA62E5E517FD7");

            entity.ToTable("Utente");

            entity.HasIndex(e => e.IdAgente, "UQ__Utente__178FE9928FC2C5F9").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Utente__AB6E61647A361E3E").IsUnique();

            entity.Property(e => e.IdUtente).HasColumnName("id_utente");
            entity.Property(e => e.Attivo)
                .HasDefaultValue(true)
                .HasColumnName("attivo");
            entity.Property(e => e.DataCreazione)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("data_creazione");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdAgente).HasColumnName("id_agente");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .HasColumnName("password_hash");
            entity.Property(e => e.Ruolo)
                .HasMaxLength(20)
                .HasColumnName("ruolo");

            entity.HasOne(d => d.IdAgenteNavigation).WithOne(p => p.Utente)
                .HasForeignKey<Utente>(d => d.IdAgente)
                .HasConstraintName("FK_Utente_Agente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
