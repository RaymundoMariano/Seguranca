using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "varchar")]
        [Required, StringLength(50)]
        public string Nome { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "varchar")]
        [Required, StringLength(100)]
        public string Senha { get; set; }

        public virtual ICollection<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
