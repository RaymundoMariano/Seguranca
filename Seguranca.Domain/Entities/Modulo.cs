using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Modulo : Entity
    {
        public Modulo()
        {
            ModulosFormulario = new HashSet<ModuloFormulario>();
            PerfisUsuario = new HashSet<PerfilUsuario>();
            RestricoesPerfil = new HashSet<RestricaoPerfil>();
            RestricoesUsuario = new HashSet<RestricaoUsuario>();
        }

        public int ModuloId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<ModuloFormulario> ModulosFormulario { get; set; }
        public virtual ICollection<PerfilUsuario> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricoesPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricoesUsuario { get; set; }
    }
}
