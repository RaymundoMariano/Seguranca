using Microsoft.EntityFrameworkCore;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC.Repositories
{
    public class FormularioEventoRepositoryEFC : RepositoryEFC<FormularioEvento>, IFormularioEventoRepository
    {
        public FormularioEventoRepositoryEFC(SegurancaContextEFC segurancaContext) : base(segurancaContext)
        {
        }        
    }
}
