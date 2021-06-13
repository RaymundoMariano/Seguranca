using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class PerfilUsuarioRepositoryEFC : RepositoryEFC<PerfilUsuario>, IPerfilUsuarioRepository
    {
        public PerfilUsuarioRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region ObterAsync
        public async Task<PerfilUsuario> ObterAsync(int usuarioId, int? moduloId)
        {
            return await _segurancaContext.PerfilUsuario.FirstOrDefaultAsync(pu =>
                pu.UsuarioId == usuarioId &&
                pu.ModuloId == moduloId);
        }
        #endregion        
    }
}
