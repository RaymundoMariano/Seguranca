using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public class _Model
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        public string Nome { get; set; }        
    }
}
