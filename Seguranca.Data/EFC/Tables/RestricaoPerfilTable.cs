using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class RestricaoPerfilTable : IEntityTypeConfiguration<RestricaoPerfil>
    {
        public void Configure(EntityTypeBuilder<RestricaoPerfil> builder)
        {
            builder.ToTable("RestricaoPerfil");

            builder.HasIndex(p => new { p.PerfilId, p.ModuloId, p.FormularioId, p.EventoId }, "IX_RestricaoPerfil")
                .IsUnique();

            builder.HasOne(d => d.Evento)
                .WithMany(p => p.RestricaoPerfil)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoPerfil_Evento");

            builder.HasOne(d => d.Formulario)
                .WithMany(p => p.RestricaoPerfil)
                .HasForeignKey(d => d.FormularioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoPerfil_Formulario");

            builder.HasOne(d => d.Modulo)
                .WithMany(p => p.RestricaoPerfil)
                .HasForeignKey(d => d.ModuloId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoPerfil_Modulo");

            builder.HasOne(d => d.Perfil)
                .WithMany(p => p.RestricaoPerfil)
                .HasForeignKey(d => d.PerfilId)
                .HasConstraintName("RestricaoPerfil_Perfil");
        }
    }
}
