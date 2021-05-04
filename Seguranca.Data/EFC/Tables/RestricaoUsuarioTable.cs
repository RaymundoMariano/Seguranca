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

            builder.HasOne(d => d.Evento)
                .WithMany(p => p.RestricaoUsuario)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Evento");

            builder.HasOne(d => d.Formulario)
                .WithMany(p => p.RestricaoUsuario)
                .HasForeignKey(d => d.FormularioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Formulario");

            builder.HasOne(d => d.Modulo)
                .WithMany(p => p.RestricaoUsuario)
                .HasForeignKey(d => d.ModuloId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RestricaoUsuario_Modulo");

            builder.HasOne(d => d.Usuario)
                .WithMany(p => p.RestricaoUsuario)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("RestricaoUsuario_Usuario");
        }
    }
}
