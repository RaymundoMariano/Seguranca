using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Tables
{
    public class RestricaoUsuarioTable : IEntityTypeConfiguration<RestricaoUsuario>
    {
        public void Configure(EntityTypeBuilder<RestricaoUsuario> builder)
        {
            builder.ToTable("RestricaoUsuario");

            builder.HasIndex(p => new { p.UsuarioId, p.ModuloId, p.FormularioId, p.EventoId }, "IX_RestricaoUsuario")
                .IsUnique();

            builder.HasOne(d => d.Evento)
                .WithMany(p => p.RestricoesUsuario)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Evento");

            builder.HasOne(d => d.Formulario)
                .WithMany(p => p.RestricoesUsuario)
                .HasForeignKey(d => d.FormularioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Formulario");

            builder.HasOne(d => d.Modulo)
                .WithMany(p => p.RestricoesUsuario)
                .HasForeignKey(d => d.ModuloId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Modulo");

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.RestricoesUsuario)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("RestricaoUsuario_Usuario");
        }
    }
}
