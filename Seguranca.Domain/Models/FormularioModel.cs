using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public partial class FormularioModel : _Model
    {
        public FormularioModel()
        {
            FormularioEvento = new HashSet<FormularioEventoModel>();
            ModuloFormulario = new HashSet<ModuloFormularioModel>();
            RestricaoPerfil = new HashSet<RestricaoPerfilModel>();
            RestricaoUsuario = new HashSet<RestricaoUsuarioModel>();
        }

        [DisplayName("Id")]
        public int FormularioId { get; set; }

        [DisplayName("Formulário")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<FormularioEventoModel> FormularioEvento { get; set; }
        public virtual ICollection<ModuloFormularioModel> ModuloFormulario { get; set; }
        public virtual ICollection<RestricaoPerfilModel> RestricaoPerfil { get; set; }
        public virtual ICollection<RestricaoUsuarioModel> RestricaoUsuario { get; set; }
    }
}
