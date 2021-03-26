using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguranca.Core.Domain.Models
{
    public class Entity
    {
        [Column(TypeName = "varchar")]
        [Required, StringLength(50)]
        public string Nome { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string Descricao { get; set; }
    }
}
