using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class UsuarioRepositoryEFC : RepositoryEFC<Usuario>, IUsuarioRepository
    {
        public UsuarioRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

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
