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

        [DisplayName("Perfil")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Descricao { get; set; }

        [DisplayName("Função")]
        [Required(ErrorMessage = "campo obrigatório")]
        public int FuncaoId { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual FuncaoModel Funcao { get; set; }
        public virtual ICollection<PerfilUsuarioModel> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricaoPerfil { get; set; }
    }
}
