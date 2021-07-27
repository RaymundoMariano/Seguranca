using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class FormularioRepositoryEFC : RepositoryEFC<Formulario>, IFormularioRepository
    {
        public FormularioRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region GetFullAsync
        public async Task<IEnumerable<Formulario>> GetFullAsync()
        {
            return await _segurancaContext.Formularios
                    .AsNoTracking()
                    .Include(f => f.FormulariosEvento)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.ModulosFormulario)
                        .ThenInclude(f => f.Modulo)
                    .Include(f => f.RestricoesPerfil)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.RestricoesUsuario)
                        .ThenInclude(f => f.Evento)
                    .ToListAsync();
        }

        public async Task<Formulario> GetFullAsync(int formularioId)
        {
            return await _segurancaContext.Formularios
                    .AsNoTracking()
                    .Include(f => f.FormulariosEvento)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.ModulosFormulario)
                        .ThenInclude(f => f.Modulo)
                    .Include(f => f.RestricoesPerfil)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.RestricoesUsuario)
                        .ThenInclude(f => f.Evento)
                    .FirstOrDefaultAsync(f => f.FormularioId == formularioId);
        }
        #endregion        
    }
}
