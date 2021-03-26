using System.Collections.Generic;

#nullable disable

namespace Seguranca.Core.Domain.Models
{
    public partial class Formulario : Entity
    {
        public Formulario()
        {
            FormularioEvento = new HashSet<FormularioEvento>();
            ModuloFormulario = new HashSet<ModuloFormulario>();
            RestricaoPerfil = new HashSet<RestricaoPerfil>();
            RestricaoUsuario = new HashSet<RestricaoUsuario>();
        }

        public int FormularioId { get; set; }
        
        public virtual ICollection<FormularioEvento> FormularioEvento { get; set; }
        public virtual ICollection<ModuloFormulario> ModuloFormulario { get; set; }
        public virtual ICollection<RestricaoPerfil> RestricaoPerfil { get; set; }
        public virtual ICollection<RestricaoUsuario> RestricaoUsuario { get; set; }
    }
}
