#nullable disable

namespace Seguranca.Core.Domain.Models
{
    public partial class RestricaoPerfil
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public int? ModuloId { get; set; }
        public int? FormularioId { get; set; }
        public int? EventoId { get; set; }

        public virtual Evento Evento { get; set; }
        public virtual Formulario Formulario { get; set; }
        public virtual Modulo Modulo { get; set; }
        public virtual Perfil Perfil { get; set; }
    }
}
