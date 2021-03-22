using Seguranca.Core.Domain.Models;
using System.Collections.Generic;

namespace Seguranca.Core.Persistence.Contexts
{
    internal class DbInitializer 
    {
        public DbInitializer(SegurancaContext context)
        {
            if (!context.Database.EnsureCreated()) return;

            var eventos = new List<Evento>() {
                new Evento() { Nome = "Incluir" },
                new Evento() { Nome = "Alterar" },
                new Evento() { Nome = "Excluir" }
            };
            context.Evento.AddRange(eventos);
            context.SaveChanges();
        }
    }
}
