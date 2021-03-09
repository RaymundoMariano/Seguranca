using System.Collections.Generic;

#nullable disable

namespace Seguranca.Core.Domain.Models
{
    public partial class Modulo
    {
        public Modulo()
        {
            ModuloFormulario = new HashSet<ModuloFormulario>();
            PerfilUsuario = new HashSet<PerfilUsuario>();
            RestricaoPerfil = new HashSet<RestricaoPerfil>();
            RestricaoUsuario = new HashSet<RestricaoUsuario>();
        }

        public int ModuloId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<ModuloFormulario> ModuloFormulario { get; set; }
        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricaoPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
