using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Books.Models;

public partial class KnjigeContext : DbContext
{
    public KnjigeContext()
    {
    }

    public KnjigeContext(DbContextOptions<KnjigeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Favoriti> Favoritis { get; set; }

    public virtual DbSet<Historija> Historijas { get; set; }

    public virtual DbSet<Knjige> Knjiges { get; set; }

    public virtual DbSet<Komentari> Komentaris { get; set; }

    public virtual DbSet<Korisnici> Korisnicis { get; set; }

    public virtual DbSet<Preporuka> Preporukas { get; set; }

    public virtual DbSet<Recenzije> Recenzijes { get; set; }

    public virtual DbSet<Zanr> Zanrs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Knjige;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.ToTable("Autor");

            entity.Property(e => e.AutorId)
                .ValueGeneratedNever()
                .HasColumnName("autor_id");
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .HasColumnName("ime");
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Favoriti>(entity =>
        {
            entity.HasKey(e => e.FavoritId);

            entity.ToTable("Favoriti");

            entity.Property(e => e.FavoritId).HasColumnName("favorit_id");
            entity.Property(e => e.KnjigaId).HasColumnName("knjiga_id");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");

            entity.HasOne(d => d.Knjiga).WithMany(p => p.Favoritis)
                .HasForeignKey(d => d.KnjigaId)
                .HasConstraintName("FK_Favoriti_Knjige");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Favoritis)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Favoriti_Korisnici");
        });

        modelBuilder.Entity<Historija>(entity =>
        {
            entity.ToTable("Historija");

            entity.Property(e => e.HistorijaId)
                .ValueGeneratedNever()
                .HasColumnName("historija_id");
            entity.Property(e => e.KnjigaId).HasColumnName("knjiga_id");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");

            entity.HasOne(d => d.Knjiga).WithMany(p => p.Historijas)
                .HasForeignKey(d => d.KnjigaId)
                .HasConstraintName("FK_Historija_Knjige1");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Historijas)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Historija_Korisnici");
        });

        modelBuilder.Entity<Knjige>(entity =>
        {
            entity.HasKey(e => e.KnjigaId);

            entity.ToTable("Knjige");

            entity.Property(e => e.KnjigaId)
                .ValueGeneratedNever()
                .HasColumnName("knjiga_id");
            entity.Property(e => e.Autor)
                .HasMaxLength(50)
                .HasColumnName("autor");
            entity.Property(e => e.AutorId).HasColumnName("autor_id");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.Naslov)
                .HasMaxLength(50)
                .HasColumnName("naslov");
            entity.Property(e => e.Opis)
                .IsUnicode(false)
                .HasColumnName("opis");
            entity.Property(e => e.SlikaUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slika_url");
            entity.Property(e => e.ZanrId).HasColumnName("zanr_id");

            entity.HasOne(d => d.AutorNavigation).WithMany(p => p.Knjiges)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("FK_Knjige_Autor");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Knjiges)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Knjige_Korisnici");

            entity.HasOne(d => d.Zanr).WithMany(p => p.Knjiges)
                .HasForeignKey(d => d.ZanrId)
                .HasConstraintName("FK_Knjige_Zanr");
        });

        modelBuilder.Entity<Komentari>(entity =>
        {
            entity.HasKey(e => e.KomentarId);

            entity.ToTable("Komentari");

            entity.Property(e => e.KomentarId)
                .ValueGeneratedNever()
                .HasColumnName("komentar_id");
            entity.Property(e => e.DatumKomentara)
                .HasColumnType("datetime")
                .HasColumnName("datum_komentara");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.RecenzijaId).HasColumnName("recenzija_id");
            entity.Property(e => e.TekstKomentara)
                .HasMaxLength(50)
                .HasColumnName("tekst_komentara");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Komentaris)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Komentari_Korisnici");

            entity.HasOne(d => d.Recenzija).WithMany(p => p.Komentaris)
                .HasForeignKey(d => d.RecenzijaId)
                .HasConstraintName("FK_Komentari_Recenzije");
        });

        modelBuilder.Entity<Korisnici>(entity =>
        {
            entity.HasKey(e => e.KorisnikId);

            entity.ToTable("Korisnici");

            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.DatumRegistracije)
                .HasColumnType("datetime")
                .HasColumnName("datum_registracije");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(50)
                .HasColumnName("ime");
            entity.Property(e => e.KorisnickoIme)
                .HasMaxLength(50)
                .HasColumnName("korisnicko_ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(50)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Preporuka>(entity =>
        {
            entity.ToTable("Preporuka");

            entity.Property(e => e.PreporukaId).HasColumnName("preporuka_id");
            entity.Property(e => e.DatumPreporuke)
                .HasColumnType("datetime")
                .HasColumnName("datum_preporuke");
            entity.Property(e => e.KnjigaId).HasColumnName("knjiga_id");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");

            entity.HasOne(d => d.Knjiga).WithMany(p => p.Preporukas)
                .HasForeignKey(d => d.KnjigaId)
                .HasConstraintName("FK_Preporuka_Knjige");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Preporukas)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Preporuka_Korisnici");
        });

        modelBuilder.Entity<Recenzije>(entity =>
        {
            entity.HasKey(e => e.RecenzijaId);

            entity.ToTable("Recenzije");

            entity.Property(e => e.RecenzijaId).HasColumnName("recenzija_id");
            entity.Property(e => e.DatumRecenzije)
                .HasColumnType("datetime")
                .HasColumnName("datum_recenzije");
            entity.Property(e => e.KnjigaId).HasColumnName("knjiga_id");
            entity.Property(e => e.Komentar)
                .HasMaxLength(50)
                .HasColumnName("komentar");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.Ocjena).HasColumnName("ocjena");

            entity.HasOne(d => d.Knjiga).WithMany(p => p.Recenzijes)
                .HasForeignKey(d => d.KnjigaId)
                .HasConstraintName("FK_Recenzije_Knjige");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Recenzijes)
                .HasForeignKey(d => d.KorisnikId)
                .HasConstraintName("FK_Recenzije_Korisnici");
        });

        modelBuilder.Entity<Zanr>(entity =>
        {
            entity.ToTable("Zanr");

            entity.Property(e => e.ZanrId)
                .ValueGeneratedNever()
                .HasColumnName("zanr_id");
            entity.Property(e => e.NazivZanra)
                .HasMaxLength(50)
                .HasColumnName("naziv_zanra");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
