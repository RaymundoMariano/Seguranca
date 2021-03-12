using Microsoft.EntityFrameworkCore;
using Seguranca.Core.Domain.Models;
using Seguranca.Core.Domain.Repositories;
using Seguranca.Core.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seguranca.Core.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SegurancaContext _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;

        public UsuarioRepository(SegurancaContext segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            return await _segurancaContext.Usuario
                    .AsNoTracking()
                    .Include(u => u.PerfilUsuario)
                        .ThenInclude(u => u.Perfil)
                    .Include(u => u.RestricaoUsuario)
                        .ThenInclude(u => u.Evento)
                            .ThenInclude(u => u.FormularioEvento)
                                .ThenInclude(u => u.Formulario)
                                    .ThenInclude(u => u.ModuloFormulario)
                                        .ThenInclude(u => u.Modulo)
                    .ToListAsync();
        }

        public async Task<Usuario> ObterAsync(int usuarioId)
        {
            return await _segurancaContext.Usuario
                    .AsNoTracking()
                    .Include(u => u.PerfilUsuario)
                        .ThenInclude(u => u.Perfil)
                    .Include(u => u.RestricaoUsuario)
                        .ThenInclude(u => u.Evento)
                            .ThenInclude(u => u.FormularioEvento)
                                .ThenInclude(u => u.Formulario)
                                    .ThenInclude(u => u.ModuloFormulario)
                                        .ThenInclude(u => u.Modulo)
                    .FirstAsync(u => u.UsuarioId == usuarioId);
        }
        #endregion

        #region Insere
        public void Insere(Usuario usuario)
        {
            _segurancaContext.Usuario.Add(usuario);
        }
        #endregion

        #region Update
        public void Update(Usuario usuario)
        {
            _segurancaContext.Entry(usuario).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(Usuario usuario)
        {
            _segurancaContext.Usuario.Remove(usuario);
        }
        #endregion

        #region Exists
        public bool Exists(int usuarioId)
        {
            return _segurancaContext.Usuario.Any(e => e.UsuarioId == usuarioId);
        }
        #endregion
    }
}
