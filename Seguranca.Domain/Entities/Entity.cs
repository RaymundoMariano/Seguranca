using System.ComponentModel.DataAnnotations.Schema;

namespace Seguranca.Domain.Entities
{
    public class Entity
    {   
        [NotMapped]
        public bool Selected { get; set; }
    }
}
