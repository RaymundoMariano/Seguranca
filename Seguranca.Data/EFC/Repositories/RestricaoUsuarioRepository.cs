using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class RestricaoUsuarioRepositoryEFC : RepositoryEFC<RestricaoUsuario>, IRestricaoUsuarioRepository
    {
        public RestricaoUsuarioRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region ObterAsync
        public async Task<RestricaoUsuario> ObterAsync(int usuarioId, int? moduloId, int? formularioId, int? eventoId)
        {
            return await _segurancaContext.RestricaoUsuario.FirstOrDefaultAsync(rp =>
                rp.UsuarioId == usuarioId &&
                rp.ModuloId == moduloId &&
                rp.FormularioId == formularioId &&
                rp.EventoId == eventoId);
        }
        #endregion        
    }
}
