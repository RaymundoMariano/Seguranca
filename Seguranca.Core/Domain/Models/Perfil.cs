using System.Collections.Generic;

#nullable disable

namespace Seguranca.Core.Domain.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            PerfilUsuario = new HashSet<PerfilUsuario>();
            RestricaoPerfil = new HashSet<RestricaoPerfil>();
        }

        public int PerfilId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricaoPerfil { get; set; }
    }
}
