using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Core.Persistence.Repositories
{
    public class ModuloRepository : IModuloRepository
    {
        private readonly SegurancaContext _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;

        public ModuloRepository(SegurancaContext segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Modulo>> ObterAsync()
        {
            return await _segurancaContext.Modulo
                    .AsNoTracking()
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricaoPerfil)
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricaoUsuario)
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .ToListAsync();
        }

        public async Task<Modulo> ObterAsync(int moduloId)
        {
            return await _segurancaContext.Modulo
                    .AsNoTracking()
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricaoPerfil)
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .Include(m => m.RestricaoUsuario)
                    .Include(m => m.ModuloFormulario)
                        .ThenInclude(m => m.Formulario)
                            .ThenInclude(m => m.FormularioEvento)
                                .ThenInclude(m => m.Evento)
                    .FirstAsync(m => m.ModuloId == moduloId);
        }
        #endregion

        #region Insere
        public void Insere(Modulo modulo)
        {
            _segurancaContext.Modulo.Add(modulo);
        }
        #endregion

        #region Update
        public void Update(Modulo modulo)
        {
            _segurancaContext.Entry(modulo).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(Modulo modulo)
        {
            _segurancaContext.Modulo.Remove(modulo);
        }
        #endregion

        #region Exists
        public bool Exists(int moduloId)
        {
            return _segurancaContext.Modulo.Any(e => e.ModuloId == moduloId);
        }
        #endregion

    }
}
