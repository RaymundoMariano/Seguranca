using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class PerfilTable : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable("Perfil");

            builder.HasIndex(e => e.Nome, "IX_Perfil")
                .IsUnique();

            builder.Property(e => e.Descricao).HasMaxLength(50);

            builder.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.FuncaoId)
                .IsRequired();

            builder.Property(e => e.CreatedSystem).HasDefaultValue(false);
        }
    }
}
