using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class EventoTable : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("Evento");

            builder.HasIndex(e => e.Nome, "IX_Evento")
                    .IsUnique();

            builder.Property(e => e.Descricao).HasMaxLength(50);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
