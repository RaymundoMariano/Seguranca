#nullable disable

using Seguranca.Core.Domain.Models;

namespace Seguranca.Core.Domain.Models
{
    public partial class FormularioEvento
    {
        public int Id { get; set; }
        public int FormularioId { get; set; }
        public int EventoId { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Formulario Formulario { get; set; }
    }
}
