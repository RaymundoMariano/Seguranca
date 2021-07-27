using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            PerfisUsuario = new HashSet<PerfilUsuario>();
            RestricoesUsuario = new HashSet<RestricaoUsuario>();
        }

        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<PerfilUsuario> PerfisUsuario { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricoesUsuario { get; set; }
    }
}
