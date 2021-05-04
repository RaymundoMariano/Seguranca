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
                new Evento() { Nome = "Incluir" },
                new Evento() { Nome = "Alterar" },
                new Evento() { Nome = "Excluir" },
                new Evento() { Nome = "Associar" },
                new Evento() { Nome = "Desassociar" }
            };
            context.Evento.AddRange(eventos);

            var formularios = new List<Formulario>() {
                new Formulario() { Nome = "FrmPesquisarEvento" },
                new Formulario() { Nome = "FrmManterEvento" },
                new Formulario() { Nome = "FrmAssociarEvento" },
                new Formulario() { Nome = "FrmPesquisarFormulario" },
                new Formulario() { Nome = "FrmManterFormulario" },
                new Formulario() { Nome = "FrmAssociarFormulario" },
                new Formulario() { Nome = "FrmPesquisarModulo" },
                new Formulario() { Nome = "FrmManterModulo" },
                new Formulario() { Nome = "FrmPesquisarPerfil" },
                new Formulario() { Nome = "FrmManterPerfil" },
                new Formulario() { Nome = "FrmAssociarPerfil" },
                new Formulario() { Nome = "FrmRestringirPerfil" },
                new Formulario() { Nome = "FrmPesquisarUsuario" },
                new Formulario() { Nome = "FrmManterUsuario" },
                new Formulario() { Nome = "FrmRestringirUsuarrio" },
                new Formulario() { Nome = "Default" },
                new Formulario() { Nome = "FrmMensagem" }
            };
            context.Formulario.AddRange(formularios);

            context.Modulo.Add(new Modulo()
            {
                Nome = "SegurancaNet",
                Descricao = "Sistema de Segurança"
            });

            var perfis = new List<Perfil>() {
                new Perfil() { Nome = "Sem Perfil" },
                new Perfil() { Nome = "Administrador" }
            };
            context.Perfil.AddRange(perfis);

            context.SaveChanges();

            //context.Usuario.Add(new Usuario()
            //{
            //    Nome = "Raymundo Gledisson",
            //    Email = "gledisson@hotmail.com",
            //});
            //context.SaveChanges();

            //context.PerfilUsuario.Add(new PerfilUsuario()
            //{
            //    UsuarioId = 1,
            //    ModuloId = 1,
            //    PerfilId = context.Perfil.First(p => p.Nome == "Administrador").PerfilId
            //});

            //context.SaveChanges();
        }
    }
}
