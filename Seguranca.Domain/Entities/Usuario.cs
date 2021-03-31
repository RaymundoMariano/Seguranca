using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Usuario : Entity
    {
        public Usuario()
        {
            PerfilUsuario = new HashSet<PerfilUsuario>();
            RestricaoUsuario = new HashSet<RestricaoUsuario>();
        }

        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
