using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Models
{
    public class UsuarioModel : _Model
    {
        [DisplayName("Id")]
        public int UsuarioId { get; set; }
        
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(50, ErrorMessage = "limite de caracteres excedido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(8, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
