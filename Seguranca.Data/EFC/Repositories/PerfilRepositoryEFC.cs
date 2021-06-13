using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class PerfilRepositoryEFC : RepositoryEFC<Perfil>, IPerfilRepository
    {
        public PerfilRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region ObterAsyncFull
        public async Task<IEnumerable<Perfil>> ObterAsyncFull()
        {
            return await _segurancaContext.Perfil
                    .AsNoTracking()
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Evento)
                    .ToListAsync();
        }

        public async Task<Perfil> ObterAsyncFull(int perfilId)
        {
            return await _segurancaContext.Perfil
                    .AsNoTracking()
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Evento)
                    .FirstOrDefaultAsync(p => p.PerfilId == perfilId);
        }
        #endregion
    }
}
