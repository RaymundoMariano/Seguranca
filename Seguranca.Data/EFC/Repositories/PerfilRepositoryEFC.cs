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

        #region GetFullAsync
        public async Task<IEnumerable<Perfil>> GetFullAsync()
        {
            return await _segurancaContext.Perfis
                    .AsNoTracking()
                    .Include(p => p.Funcao)
                    .Include(p => p.PerfisUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Evento)
                    .ToListAsync();
        }

        public async Task<Perfil> GetFullAsync(int perfilId)
        {
            return await _segurancaContext.Perfis
                    .AsNoTracking()
                    .Include(p => p.Funcao)
                    .Include(p => p.PerfisUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Evento)
                    .FirstOrDefaultAsync(p => p.PerfilId == perfilId);
        }

        public async Task<Perfil> GetFullAsync(string nome)
        {
            return await _segurancaContext.Perfis
                    .AsNoTracking()
                    .Include(p => p.Funcao)
                    .Include(p => p.PerfisUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricoesPerfil)
                        .ThenInclude(p => p.Evento)
                    .FirstOrDefaultAsync(p => p.Nome == nome);
        }
        #endregion
    }
}
