using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;

namespace Seguranca.Data.EFC.Repositories
{
    public class ModuloFormularioRepositoryEFC : RepositoryEFC<ModuloFormulario>, IModuloFormularioRepository
    {
        public ModuloFormularioRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }
    }
}
