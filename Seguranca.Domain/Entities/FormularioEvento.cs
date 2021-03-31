using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seguranca.Domain.Entities
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
