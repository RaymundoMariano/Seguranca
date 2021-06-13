using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class FormularioTable : IEntityTypeConfiguration<Formulario>
    {
        public void Configure(EntityTypeBuilder<Formulario> builder)
        {
            builder.ToTable("Formulario");

            builder.HasIndex(e => e.Nome, "IX_Formulario")
                .IsUnique();

            builder.Property(e => e.Descricao).HasMaxLength(50);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CreatedSystem).HasDefaultValue(false);
        }
    }
}
