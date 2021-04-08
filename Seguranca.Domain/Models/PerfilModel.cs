using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public class PerfilModel : _Model
    {
        [DisplayName("Id")]
        public int PerfilId { get; set; }

        [DisplayName("Descrição")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Descricao { get; set; }
    }
}
