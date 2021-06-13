using Seguranca.Domain.Entities;
using System.Collections.Generic;

namespace Seguranca.Data.EFC
{
    internal class DbInitializer
    {
        public DbInitializer(SegurancaContextEFC context)
        {
            if (!context.Database.EnsureCreated()) return;

            var eventos = new List<Evento>() {
                new Evento() { Nome = "Incluir", CreatedSystem = true },
                new Evento() { Nome = "Alterar", CreatedSystem = true },
                new Evento() { Nome = "Excluir", CreatedSystem = true },
            };
            context.Evento.AddRange(eventos);

            var formularios = new List<Formulario>() {
                new Formulario() { Nome = "Evento", CreatedSystem = true },
                new Formulario() { Nome = "Formulario", CreatedSystem = true },
                new Formulario() { Nome = "Modulo", CreatedSystem = true },
                new Formulario() { Nome = "Perfil", CreatedSystem = true },
                new Formulario() { Nome = "Usuario", CreatedSystem = true },
                new Formulario() { Nome = "UsuarioPerfil", CreatedSystem = true },
            };
            context.Formulario.AddRange(formularios);

            context.Modulo.Add(new Modulo()
            {
                Nome = "SegurancaNet",
                Descricao = "Sistema de Segurança",
                CreatedSystem = true
            });

            var perfis = new List<Perfil>() {
                new Perfil() { Nome = "Sem Perfil", CreatedSystem = true },
                new Perfil() { Nome = "Administrador", CreatedSystem = true }
            };
            context.Perfil.AddRange(perfis);

            context.SaveChanges();
        }
    }
}
