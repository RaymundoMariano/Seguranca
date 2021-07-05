using System.ComponentModel.DataAnnotations;

namespace Seguranca.Domain
{
    public class SegurancaModel
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int ModuloId { get; set; }

        [Required]
        public int PerfilId { get; set; }
    }
}
