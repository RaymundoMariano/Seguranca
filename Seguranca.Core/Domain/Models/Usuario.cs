using System.Collections.Generic;

#nullable disable

namespace Seguranca.Core.Domain.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            PerfilUsuario = new HashSet<PerfilUsuario>();
            RestricaoUsuario = new HashSet<RestricaoUsuario>();
        }

        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
