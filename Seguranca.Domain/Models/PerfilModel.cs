using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public partial class PerfilModel : _Model
    {
        public PerfilModel()
        {
            PerfilUsuario = new HashSet<PerfilUsuarioModel>();
            RestricaoPerfil = new HashSet<RestricaoPerfilModel>();
        }

        [DisplayName("Id")]
        public int PerfilId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Descricao { get; set; }

        public virtual ICollection<PerfilUsuarioModel> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricaoPerfil { get; set; }
    }
}
