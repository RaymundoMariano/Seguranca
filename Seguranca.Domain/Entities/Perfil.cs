using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Perfil : Entity
    {
        public Perfil()
        {
            PerfisUsuario = new HashSet<PerfilUsuario>();
            RestricoesPerfil = new HashSet<RestricaoPerfil>();
        }

        public int PerfilId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int FuncaoId { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual Funcao Funcao { get; set; }
        public virtual ICollection<PerfilUsuario> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricoesPerfil { get; set; }
    }
}
