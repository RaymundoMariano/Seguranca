using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public class ModuloModel : _Model
    {
        public ModuloModel()
        {
            ModuloFormulario = new HashSet<ModuloFormularioModel>();
            PerfilUsuario = new HashSet<PerfilUsuarioModel>();
            RestricaoPerfil = new HashSet<RestricaoPerfilModel>();
            RestricaoUsuario = new HashSet<RestricaoUsuarioModel>();
        }

        [DisplayName("Id")]
        public int ModuloId { get; set; }

        [DisplayName("Módulo")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<ModuloFormularioModel> ModuloFormulario { get; set; }
        public virtual ICollection<PerfilUsuarioModel> PerfilUsuario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricaoPerfil { get; set; }
        public virtual ICollection<RestricaoUsuarioModel> RestricaoUsuario { get; set; }
    }
}
