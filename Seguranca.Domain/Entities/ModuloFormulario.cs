namespace Seguranca.Domain.Entities
{
    public partial class ModuloFormulario
    {
        public int Id { get; set; }
        public int ModuloId { get; set; }
        public int FormularioId { get; set; }

        public virtual Formulario Formulario { get; set; }
        public virtual Modulo Modulo { get; set; }
    }
}
