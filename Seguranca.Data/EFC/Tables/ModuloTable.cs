using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class ModuloTable : IEntityTypeConfiguration<Modulo>
    {
        public void Configure(EntityTypeBuilder<Modulo> builder)
        {
            builder.ToTable("Modulo");

            builder.HasIndex(e => e.Nome, "IX_Modulo")
                .IsUnique();

            builder.Property(e => e.Descricao).HasMaxLength(50);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
