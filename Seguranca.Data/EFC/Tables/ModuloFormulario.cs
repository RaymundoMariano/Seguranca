using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class ModuloFormularioTable : IEntityTypeConfiguration<ModuloFormulario>
    {
        public void Configure(EntityTypeBuilder<ModuloFormulario> builder)
        {
            builder.ToTable("ModuloFormulario");

            builder.HasIndex(e => new { e.ModuloId, e.FormularioId }, "IX_ModuloFormulario")
                .IsUnique();

            builder.HasOne(d => d.Formulario)
                .WithMany(p => p.ModuloFormulario)
                .HasForeignKey(d => d.FormularioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModuloFormulario_Formulario");

            builder.HasOne(d => d.Modulo)
                .WithMany(p => p.ModuloFormulario)
                .HasForeignKey(d => d.ModuloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ModuloFormulario_Modulo");

            builder.Property(e => e.CreatedSystem).HasDefaultValue(false);
        }
    }
}
