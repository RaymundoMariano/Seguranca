using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC
{
    public partial class SegurancaContextEFC : DbContext, IUnitOfWork
    {
        public SegurancaContextEFC()
        {
        }

        public SegurancaContextEFC(DbContextOptions<SegurancaContextEFC> options)
            : base(options)
        {
            new DbInitializer(this);
        }

        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Formulario> Formulario { get; set; }
        public virtual DbSet<FormularioEvento> FormularioEvento { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<ModuloFormulario> ModuloFormulario { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual DbSet<RestricaoPerfil> RestricaoPerfil { get; set; }
        public virtual DbSet<RestricaoUsuario> RestricaoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=DESKTOP-S3R5UB7\\SQLEXPRESS;Database=DesBd_Seguranca;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.ToTable("Evento");

                entity.HasIndex(e => e.Nome, "IX_Evento")
                    .IsUnique();

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Formulario>(entity =>
            {
                entity.ToTable("Formulario");

                entity.HasIndex(e => e.Nome, "IX_Formulario")
                    .IsUnique();

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FormularioEvento>(entity =>
            {
                entity.ToTable("FormularioEvento");

                entity.HasIndex(e => new { e.FormularioId, e.EventoId }, "IX_FormularioEvento")
                    .IsUnique();

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.FormularioEvento)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormularioEvento_Evento");

                entity.HasOne(d => d.Formulario)
                    .WithMany(p => p.FormularioEvento)
                    .HasForeignKey(d => d.FormularioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormularioEvento_Formulario");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.ToTable("Modulo");

                entity.HasIndex(e => e.Nome, "IX_Modulo")
                    .IsUnique();

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ModuloFormulario>(entity =>
            {
                entity.ToTable("ModuloFormulario");

                entity.HasIndex(e => new { e.ModuloId, e.FormularioId }, "IX_ModuloFormulario")
                    .IsUnique();

                entity.HasOne(d => d.Formulario)
                    .WithMany(p => p.ModuloFormulario)
                    .HasForeignKey(d => d.FormularioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuloFormulario_Formulario");

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.ModuloFormulario)
                    .HasForeignKey(d => d.ModuloId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuloFormulario_Modulo");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("Perfil");

                entity.HasIndex(e => e.Nome, "IX_Perfil")
                    .IsUnique();

                entity.Property(e => e.Descricao).HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PerfilUsuario>(entity =>
            {
                entity.ToTable("PerfilUsuario");

                entity.HasIndex(e => new { e.UsuarioId, e.ModuloId, e.PerfilId }, "IX_PerfilUsuario")
                    .IsUnique();

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.PerfilUsuario)
                    .HasForeignKey(d => d.ModuloId)
                    .HasConstraintName("PerfilUsuario_Modulo");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.PerfilUsuario)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PerfilUsuario_Perfil");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.PerfilUsuario)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("PerfilUsuario_Usuario");
            });

            modelBuilder.Entity<RestricaoPerfil>(entity =>
            {
                entity.ToTable("RestricaoPerfil");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.RestricaoPerfil)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoPerfil_Evento");

                entity.HasOne(d => d.Formulario)
                    .WithMany(p => p.RestricaoPerfil)
                    .HasForeignKey(d => d.FormularioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoPerfil_Formulario");

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.RestricaoPerfil)
                    .HasForeignKey(d => d.ModuloId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoPerfil_Modulo");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.RestricaoPerfil)
                    .HasForeignKey(d => d.PerfilId)
                    .HasConstraintName("RestricaoPerfil_Perfil");
            });

            modelBuilder.Entity<RestricaoUsuario>(entity =>
            {
                entity.ToTable("RestricaoUsuario");

                entity.HasOne(d => d.Evento)
                    .WithMany(p => p.RestricaoUsuario)
                    .HasForeignKey(d => d.EventoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoUsuario_Evento");

                entity.HasOne(d => d.Formulario)
                    .WithMany(p => p.RestricaoUsuario)
                    .HasForeignKey(d => d.FormularioId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoUsuario_Formulario");

                entity.HasOne(d => d.Modulo)
                    .WithMany(p => p.RestricaoUsuario)
                    .HasForeignKey(d => d.ModuloId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("RestricaoUsuario_Modulo");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.RestricaoUsuario)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("RestricaoUsuario_Usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasIndex(e => e.Nome, "IX_Usuario")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "IX_Usuario_1")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
