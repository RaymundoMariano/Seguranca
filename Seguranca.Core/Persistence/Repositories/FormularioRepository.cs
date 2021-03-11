using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Core.Persistence.Repositories
{
    public class FormularioRepository : IFormularioRepository
    {
        private readonly SegurancaContext _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;

        public FormularioRepository(SegurancaContext segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Formulario>> ObterAsync()
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

        public async Task<Formulario> ObterAsync(int formularioId)
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

        #region Insere
        public void Insere(Formulario formulario)
        {
            _segurancaContext.Formulario.Add(formulario);
        }
        #endregion

        #region Update
        public void Update(Formulario formulario)
        {
            _segurancaContext.Entry(formulario).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(Formulario formulario)
        {
            _segurancaContext.Formulario.Remove(formulario);
        }
        #endregion

        #region Exists
        public bool Exists(int formularioId)
        {
            return _segurancaContext.Formulario.Any(e => e.FormularioId == formularioId);
        }
        #endregion
    }
}
