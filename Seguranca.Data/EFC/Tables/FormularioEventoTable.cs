using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class FormularioEventoTable : IEntityTypeConfiguration<FormularioEvento>
    {
        public void Configure(EntityTypeBuilder<FormularioEvento> builder)
        {
            builder.ToTable("FormularioEvento");

            builder.HasIndex(e => new { e.FormularioId, e.EventoId }, "IX_FormularioEvento")
                .IsUnique();

            builder.HasOne(d => d.Evento)
                .WithMany(p => p.FormulariosEvento)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormularioEvento_Evento");

            builder.HasOne(d => d.Formulario)
                .WithMany(p => p.FormulariosEvento)
                .HasForeignKey(d => d.FormularioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FormularioEvento_Formulario");

            builder.Property(e => e.CreatedSystem).HasDefaultValue(false);
        }
    }
}
