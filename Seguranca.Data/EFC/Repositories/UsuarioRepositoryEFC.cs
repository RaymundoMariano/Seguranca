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

        #region GetFullAsync
        public async Task<IEnumerable<Usuario>> GetFullAsync()
        {
            return await _segurancaContext.Usuario
                    //.AsNoTracking()
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Perfil)
                        .ThenInclude(p => p.Funcao)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Evento)
                    .ToListAsync();
        }

        public async Task<Usuario> GetFullAsync(int usuarioId)
        {
            return await _segurancaContext.Usuario
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Perfil)
                        .ThenInclude(p => p.Funcao)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Evento)
                    .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);
        }

        public async Task<Usuario> GetFullAsync(string nome)
        {
            return await _segurancaContext.Usuario
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Usuario)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.PerfilUsuario)
                        .ThenInclude(p => p.Perfil)
                        .ThenInclude(p => p.Funcao)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Modulo)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Formulario)
                    .Include(p => p.RestricaoUsuario)
                        .ThenInclude(p => p.Evento)
                    .FirstOrDefaultAsync(u => u.Nome == nome);
        }
        #endregion

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            return await _segurancaContext.Set<Usuario>().ToListAsync();
        }

        public async Task<Usuario> ObterAsync(int id)
        {
            return await _segurancaContext.Set<Usuario>().FindAsync(id);
        }

        public async Task<Usuario> ObterAsync(string email)
        {
            return await _segurancaContext.Usuario.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }
        #endregion

        #region Insere
        public void Insere(Usuario usuario)
        {
            _segurancaContext.Set<Usuario>().Add(usuario);
        }
        #endregion                
    }
}
