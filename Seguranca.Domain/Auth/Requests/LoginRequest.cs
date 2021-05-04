using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain.Auth.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(100, ErrorMessage = "limite de caracteres excedido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "campo obrigatório")]
        [StringLength(40, ErrorMessage = "limite de caracteres excedido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
