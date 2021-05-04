using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class UsuarioRepositoryEFC : IUsuarioRepository
    {
        protected readonly SegurancaContextEFC _segurancaContext;
        public IUnitOfWork UnitOfWork => _segurancaContext;
        public UsuarioRepositoryEFC(SegurancaContextEFC segurancaContext)
        {
            _segurancaContext = segurancaContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            return await _segurancaContext.Set<Usuario>().ToListAsync();
        }

        public async Task<Usuario> ObterAsync(int id)
        {
            return await _segurancaContext.Set<Usuario>().FindAsync(id);
        }
        #endregion

        #region Insere
        public void Insere(Usuario usuario)
        {
            _segurancaContext.Set<Usuario>().Add(usuario);
        }
        #endregion


        #region ObterAsync
        public async Task<Usuario> ObterAsync(string email)
        {
            return await _segurancaContext.Usuario.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        #endregion

        #region ObterAsyncFull
        public async Task<IEnumerable<Usuario>> ObterAsyncFull()
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

        public async Task<Usuario> ObterAsyncFull(int usuarioId)
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
    }
}
