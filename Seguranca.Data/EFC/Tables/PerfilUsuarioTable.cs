using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class PerfilUsuarioTable : IEntityTypeConfiguration<PerfilUsuario>
    {
        public void Configure(EntityTypeBuilder<PerfilUsuario> builder)
        {
            builder.ToTable("PerfilUsuario");

            builder.HasIndex(e => new { e.UsuarioId, e.ModuloId, e.PerfilId }, "IX_PerfilUsuario")
                .IsUnique();

            builder.HasOne(d => d.Modulo)
                .WithMany(p => p.PerfisUsuario)
                .HasForeignKey(d => d.ModuloId)
                .HasConstraintName("PerfilUsuario_Modulo");

            builder.HasOne(d => d.Perfil)
                .WithMany(p => p.PerfisUsuario)
                .HasForeignKey(d => d.PerfilId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PerfilUsuario_Perfil");

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.PerfisUsuario)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("PerfilUsuario_Usuario");

            builder.Property(e => e.CreatedSystem).HasDefaultValue(false);
        }
    }
}
