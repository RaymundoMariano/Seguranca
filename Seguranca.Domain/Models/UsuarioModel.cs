using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public partial class UsuarioModel : _Model
    {
        public UsuarioModel()
        {
            PerfilUsuario = new HashSet<PerfilUsuarioModel>();
            RestricaoUsuario = new HashSet<RestricaoUsuarioModel>();
        }

        [DisplayName("Id")]
        public int UsuarioId { get; set; }

        [DisplayName("Usuário")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        [EmailAddress]
        public string Email { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<PerfilUsuarioModel> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoUsuarioModel> RestricaoUsuario { get; set; }
    }
}
