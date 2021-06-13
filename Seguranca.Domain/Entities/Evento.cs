using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Evento : Entity
    {
        public Evento()
        {
            FormularioEvento = new HashSet<FormularioEvento>();
            RestricaoPerfil = new HashSet<RestricaoPerfil>();
            RestricaoUsuario = new HashSet<RestricaoUsuario>();
        }

        public int EventoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<FormularioEvento> FormularioEvento { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricaoPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
