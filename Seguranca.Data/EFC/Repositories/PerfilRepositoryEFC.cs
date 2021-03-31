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
                        .ThenInclude(p => p.Evento)
                            .ThenInclude(p => p.FormularioEvento)
                                .ThenInclude(p => p.Formulario)
                                    .ThenInclude(p => p.ModuloFormulario)
                                        .ThenInclude(p => p.Modulo)
                    .ToListAsync();
        }

        public async Task<Perfil> ObterAsyncFull(int perfilId)
        {
            return await _segurancaContext.Perfil
                    .AsNoTracking()
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricaoPerfil)
                        .ThenInclude(p => p.Evento)
                            .ThenInclude(p => p.FormularioEvento)
                                .ThenInclude(p => p.Formulario)
                                    .ThenInclude(p => p.ModuloFormulario)
                                        .ThenInclude(p => p.Modulo)
                    .FirstAsync(p => p.PerfilId == perfilId);
        }
        #endregion
    }
}
