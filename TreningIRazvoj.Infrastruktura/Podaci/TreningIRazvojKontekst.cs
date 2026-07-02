using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreningIRazvoj.Domen.Modeli;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TreningIRazvoj.Infrastruktura.Identitet;

namespace TreningIRazvoj.Infrastruktura.Podaci
{
    public class TreningIRazvojKontekst
    : IdentityDbContext<Korisnik, IdentityRole, string>
    {
        public TreningIRazvojKontekst(
            DbContextOptions<TreningIRazvojKontekst> opcije)
            : base(opcije)
        {
        }

        public DbSet<Zaposleni> Zaposleni { get; set; }

        public DbSet<Predavac> Predavaci { get; set; }

        public DbSet<KategorijaPrograma> KategorijePrograma { get; set; }

        public DbSet<RazvojniProgram> RazvojniProgrami { get; set; }

        public DbSet<Prijava> Prijave { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prijava>()
                .HasKey(p => new
                {
                    p.ZaposleniId,
                    p.RazvojniProgramId
                });

            modelBuilder.Entity<Prijava>()
                .HasOne(p => p.Zaposleni)
                .WithMany(z => z.Prijave)
                .HasForeignKey(p => p.ZaposleniId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prijava>()
                .HasOne(p => p.RazvojniProgram)
                .WithMany(rp => rp.Prijave)
                .HasForeignKey(p => p.RazvojniProgramId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RazvojniProgram>()
                .HasOne(rp => rp.KategorijaPrograma)
                .WithMany(kp => kp.RazvojniProgrami)
                .HasForeignKey(rp => rp.KategorijaProgramaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RazvojniProgram>()
                .HasOne(rp => rp.Predavac)
                .WithMany(p => p.RazvojniProgrami)
                .HasForeignKey(rp => rp.PredavacId)
                .OnDelete(DeleteBehavior.Restrict);


            //-----------------------------------------


            modelBuilder.Entity<KategorijaPrograma>()
                .Property(kp => kp.Naziv)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<KategorijaPrograma>()
                .Property(kp => kp.Opis)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<KategorijaPrograma>()
                .HasIndex(kp => kp.Naziv)
                .IsUnique();


            modelBuilder.Entity<Predavac>()
                .Property(p => p.Ime)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Predavac>()
                .Property(p => p.Prezime)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Predavac>()
                .Property(p => p.Imejl)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Predavac>()
                .Property(p => p.OblastStrucnosti)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Predavac>()
                .HasIndex(p => p.Imejl)
                .IsUnique();


            modelBuilder.Entity<Zaposleni>()
                .Property(z => z.Ime)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Zaposleni>()
                .Property(z => z.Prezime)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Zaposleni>()
                .Property(z => z.Imejl)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Zaposleni>()
                .Property(z => z.RadnoMesto)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Zaposleni>()
                .Property(z => z.Odeljenje)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Zaposleni>()
                .HasIndex(z => z.Imejl)
                .IsUnique();


            modelBuilder.Entity<RazvojniProgram>()
                .Property(rp => rp.Naziv)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<RazvojniProgram>()
                .Property(rp => rp.Opis)
                .IsRequired()
                .HasMaxLength(1000);


            modelBuilder.Entity<Prijava>()
                .Property(p => p.ProcenatPrisustva)
                .HasPrecision(5, 2);






        }















    }
}
