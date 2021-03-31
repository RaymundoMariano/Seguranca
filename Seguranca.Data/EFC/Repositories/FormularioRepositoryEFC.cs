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

        #region ObterAsyncFull
        public async Task<IEnumerable<Formulario>> ObterAsyncFull()
        {
            return await _segurancaContext.Formulario
                    .AsNoTracking()
                    .Include(f => f.FormularioEvento)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.ModuloFormulario)
                        .ThenInclude(f => f.Modulo)
                    .Include(f => f.RestricaoPerfil)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.RestricaoUsuario)
                        .ThenInclude(f => f.Evento)
                    .ToListAsync();
        }

        public async Task<Formulario> ObterAsyncFull(int formularioId)
        {
            return await _segurancaContext.Formulario
                    .AsNoTracking()
                    .Include(f => f.FormularioEvento)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.ModuloFormulario)
                        .ThenInclude(f => f.Modulo)
                    .Include(f => f.RestricaoPerfil)
                        .ThenInclude(f => f.Evento)
                    .Include(f => f.RestricaoUsuario)
                        .ThenInclude(f => f.Evento)
                    .FirstAsync(f => f.FormularioId == formularioId);
        }
        #endregion
    }
}
