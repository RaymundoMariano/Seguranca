using System.Collections.Generic;

namespace Seguranca.Domain.Entities
{
    public partial class Evento : Entity
    {
        public Evento()
        {
            FormulariosEvento = new HashSet<FormularioEvento>();
            RestricoesPerfil = new HashSet<RestricaoPerfil>();
            RestricoesUsuario = new HashSet<RestricaoUsuario>();
        }

        public int EventoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual ICollection<FormularioEvento> FormulariosEvento { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricoesPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricoesUsuario { get; set; }
    }
}
