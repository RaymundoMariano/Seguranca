using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class RestricaoPerfilRepositoryEFC : RepositoryEFC<RestricaoPerfil>, IRestricaoPerfilRepository
    {
        public RestricaoPerfilRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }

        #region ObterAsync
        public async Task<RestricaoPerfil> ObterAsync(int perfilId, int? moduloId, int? formularioId, int? eventoId)
        {
            return await _segurancaContext.RestricaoPerfil.FirstOrDefaultAsync(rp =>
                rp.PerfilId == perfilId && 
                rp.ModuloId == moduloId && 
                rp.FormularioId == formularioId &&
                rp.EventoId == eventoId);
        }
        #endregion        
    }
}
