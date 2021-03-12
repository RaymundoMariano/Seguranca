using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Core.Persistence.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly SegurancaContext _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;

        public PerfilRepository(SegurancaContext segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Perfil>> ObterAsync()
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

        public async Task<Perfil> ObterAsync(int perfilId)
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
                    .FirstAsync(e => e.PerfilId == perfilId);
        }
        #endregion

        #region Insere
        public void Insere(Perfil perfil)
        {
            _segurancaContext.Perfil.Add(perfil);
        }
        #endregion

        #region Update
        public void Update(Perfil perfil)
        {
            _segurancaContext.Entry(perfil).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(Perfil perfil)
        {
            _segurancaContext.Perfil.Remove(perfil);
        }
        #endregion

        #region Exists
        public bool Exists(int perfilId)
        {
            return _segurancaContext.Perfil.Any(e => e.PerfilId == perfilId);
        }
        #endregion
    }
}
