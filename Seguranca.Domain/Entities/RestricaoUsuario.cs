namespace Seguranca.Domain.Entities
{
    public partial class RestricaoUsuario : Entity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? ModuloId { get; set; }
        public int? FormularioId { get; set; }
        public int? EventoId { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Formulario Formulario { get; set; }
        public virtual Modulo Modulo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
