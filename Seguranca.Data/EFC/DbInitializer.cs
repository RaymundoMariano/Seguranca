using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
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
                new Evento() { Nome = "Associar Evento", CreatedSystem = true },
                new Evento() { Nome = "Associar Formulario", CreatedSystem = true },
                new Evento() { Nome = "Associar Perfil", CreatedSystem = true },
                new Evento() { Nome = "Associar Restricoes", CreatedSystem = true }
            };
            context.Eventos.AddRange(eventos);

            var formularios = new List<Formulario>() {
                new Formulario() { Nome = "Evento", CreatedSystem = true },
                new Formulario() { Nome = "Formulario", CreatedSystem = true },
                new Formulario() { Nome = "Modulo", CreatedSystem = true },
                new Formulario() { Nome = "Perfil", CreatedSystem = true },
                new Formulario() { Nome = "Usuario", CreatedSystem = true }
            };
            context.Formularios.AddRange(formularios);
            context.SaveChanges();
            
            var f1 = new List<FormularioEvento>() {
                new FormularioEvento() { FormularioId = 1, EventoId = 3,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 1, EventoId = 2,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 1, EventoId = 1,  CreatedSystem = true }
            };
            context.FormulariosEvento.AddRange(f1);
            context.SaveChanges();

            var f2 = new List<FormularioEvento>() {
                new FormularioEvento() { FormularioId = 2, EventoId = 4,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 2, EventoId = 3,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 2, EventoId = 2,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 2, EventoId = 1,  CreatedSystem = true }
            };
            context.FormulariosEvento.AddRange(f2);
            context.SaveChanges();

            var f3 = new List<FormularioEvento>() {
                new FormularioEvento() { FormularioId = 3, EventoId = 5,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 3, EventoId = 3,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 3, EventoId = 2,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 3, EventoId = 1,  CreatedSystem = true }
            };
            context.FormulariosEvento.AddRange(f3);
            context.SaveChanges();

            var f4 = new List<FormularioEvento>() {
                new FormularioEvento() { FormularioId = 4, EventoId = 7,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 4, EventoId = 3,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 4, EventoId = 2,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 4, EventoId = 1,  CreatedSystem = true }
            };
            context.FormulariosEvento.AddRange(f4);
            context.SaveChanges();

            var f5 = new List<FormularioEvento>() {
                new FormularioEvento() { FormularioId = 5, EventoId = 7,  CreatedSystem = true },
                new FormularioEvento() { FormularioId = 5, EventoId = 6,  CreatedSystem = true }
            };
            context.FormulariosEvento.AddRange(f5);
            context.SaveChanges();

            context.Modulos.Add(new Modulo() {
                Nome = "SegurancaNet", Descricao = "Sistema de Segurança", CreatedSystem = true
            });
            context.SaveChanges();

            var mf = new List<ModuloFormulario>() {
                new ModuloFormulario() { ModuloId = 1, FormularioId = 5, CreatedSystem = true },
                new ModuloFormulario() { ModuloId = 1, FormularioId = 4, CreatedSystem = true },
                new ModuloFormulario() { ModuloId = 1, FormularioId = 3, CreatedSystem = true },
                new ModuloFormulario() { ModuloId = 1, FormularioId = 2, CreatedSystem = true },
                new ModuloFormulario() { ModuloId = 1, FormularioId = 1, CreatedSystem = true },
            };
            context.ModulosFormulario.AddRange(mf);
            context.SaveChanges();

            var funcoes = new List<Funcao>() {
                new Funcao() { Nome = "Aprendiz", CreatedSystem = true },
                new Funcao() { Nome = "Estagiário", CreatedSystem = true },
                new Funcao() { Nome = "Auxiliar", CreatedSystem = true },
                new Funcao() { Nome = "Assistente", CreatedSystem = true },
                new Funcao() { Nome = "Analista", CreatedSystem = true },
                new Funcao() { Nome = "Supervisão", CreatedSystem = true },
                new Funcao() { Nome = "Coordenação", CreatedSystem = true },
                new Funcao() { Nome = "Gerência", CreatedSystem = true },
                new Funcao() { Nome = "Diretoria", CreatedSystem = true },
                new Funcao() { Nome = "Presidência", CreatedSystem = true }
            };
            context.Funcoes.AddRange(funcoes);

            var perfis = new List<Perfil>() {
                new Perfil() { Nome = "Sem Perfil", FuncaoId = (int)EFuncao.Aprendiz, CreatedSystem = true },
                new Perfil() { Nome = "Master", FuncaoId = (int)EFuncao.Presidência, CreatedSystem = true }
            };
            context.Perfis.AddRange(perfis);
            context.SaveChanges();
        }
    }
}
