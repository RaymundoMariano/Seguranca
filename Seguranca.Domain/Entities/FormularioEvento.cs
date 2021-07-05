namespace Seguranca.Domain.Entities
{
    public partial class FormularioEvento : Entity
    {
        public int Id { get; set; }
        public int FormularioId { get; set; }
        public int EventoId { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Formulario Formulario { get; set; }
    }
}
