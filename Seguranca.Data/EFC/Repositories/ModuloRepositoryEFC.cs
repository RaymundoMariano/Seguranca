using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class ModuloRepositoryEFC : RepositoryEFC<Modulo>, IModuloRepository
    {
        public ModuloRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region GetFullAsync
        public async Task<IEnumerable<Modulo>> GetFullAsync()
        {
            return await _segurancaContext.Modulos
                    .AsNoTracking()
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesPerfil)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesUsuario)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .ToListAsync();
        }

        public async Task<Modulo> GetFullAsync(int moduloId)
        {
            return await _segurancaContext.Modulos
                    .AsNoTracking()
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesPerfil)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesUsuario)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .FirstOrDefaultAsync(m => m.ModuloId == moduloId);
        }

        public async Task<Modulo> GetFullAsync(string nome)
        {
            return await _segurancaContext.Modulos
                    .AsNoTracking()
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesPerfil)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricoesUsuario)
                    .Include(m => m.ModulosFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormulariosEvento)
                                .ThenInclude(m => m.Evento)
                    .FirstOrDefaultAsync(m => m.Nome == nome);
        }
        #endregion
    }
}
