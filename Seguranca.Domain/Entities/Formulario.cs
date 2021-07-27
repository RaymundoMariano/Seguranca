using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Formulario : Entity
    {
        public Formulario()
        {
            FormulariosEvento = new HashSet<FormularioEvento>();
            ModulosFormulario = new HashSet<ModuloFormulario>();
            RestricoesPerfil = new HashSet<RestricaoPerfil>();
            RestricoesUsuario = new HashSet<RestricaoUsuario>();
        }

        public int FormularioId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<FormularioEvento> FormulariosEvento { get; set; }
        public virtual ICollection<ModuloFormulario> ModulosFormulario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricoesPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricoesUsuario { get; set; }
    }
}
