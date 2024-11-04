using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Data
{
    public class ClinicaVeterinariaContext : IdentityDbContext<ApplicationUser>
    {
        public ClinicaVeterinariaContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Proprietario> Proprietarios { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Secretario> Secretarios { get; set; }
        public DbSet<Encontro> Encontros { get; set; }
        public DbSet<Tratamento> Tratamentos { get; set; }
        public DbSet<Receita> Medicacoes { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pet>()
               .HasRequired(p => p.Proprietario)
               .WithMany(o => o.Pets)
               .HasForeignKey(p => p.ProprietarioId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Encontro>()
             .HasRequired(a => a.Pet)
             .WithMany(p => p.Encontros)
             .HasForeignKey(a => a.PetId)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<Encontro>()
              .HasRequired(a => a.Veterinario)
              .WithMany(v => v.Encontros)
              .HasForeignKey(a => a.VeterinarioId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tratamento>()
               .HasRequired(t => t.Pet)
               .WithMany(p => p.Tratamentos)
               .HasForeignKey(t => t.PetId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tratamento>()
               .HasRequired(t => t.Encontro)
               .WithMany(a => a.Tratamentos)
               .HasForeignKey(t => t.EncontroId)
               .WillCascadeOnDelete(false);

            // Relacionamento muitos para muitos entre Tratamento e Medicação
            modelBuilder.Entity<Tratamento>()
               .HasMany(t => t.Medicacoes)
               .WithMany(m => m.Tratamentos);

            modelBuilder.Entity<Agendamento>()
               .HasRequired(s => s.Veterinario)
               .WithMany(v => v.Agendamentos)
               .HasForeignKey(s => s.VeterinarioId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Agendamento>()
            .Property(e => e.DataInicio)
            .HasColumnType("datetime");

            modelBuilder.Entity<Agendamento>()
                .Property(e => e.DataFim)
                .HasColumnType("datetime");

            modelBuilder.Entity<Secretario>();
        }

        public static ClinicaVeterinariaContext Create()
        {
            return new ClinicaVeterinariaContext();
        }

        public System.Data.Entity.DbSet<ClinicaVeterinaria.Models.Curioso> Curiosoes { get; set; }
    }
}