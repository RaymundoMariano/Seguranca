using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Service
{
    public class FuncaoService : IFuncaoService
    {
        private readonly IFuncaoRepository _funcaoRepository;
        public FuncaoService(IFuncaoRepository funcaoRepository)
        {
            _funcaoRepository = funcaoRepository;
        }

        #region GetFullAsync
        public async Task<IEnumerable<Funcao>> GetFullAsync()
        {
            try
            {
                return await _funcaoRepository.GetFullAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Funcao> GetFullAsync(int funcaoId)
        {
            try
            {
                var funcao = await _funcaoRepository.GetFullAsync(funcaoId);
                if (funcao == null)
                    throw new ServiceException($"Função com Id = {funcaoId} não foi encontrado");
                return funcao;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<IEnumerable<Funcao>> GetFullAsync(EFuncao eFuncaoInicial, EFuncao eFuncaoFinal)
        {
            try
            {
                if (eFuncaoInicial > eFuncaoFinal) throw new ServiceException(
                    $"Hierarquia inválida! {eFuncaoInicial} / {eFuncaoFinal}");

                var funcoes = await _funcaoRepository.GetFullAsync();

                funcoes = funcoes.Where(f =>
                    f.FuncaoId > (int)eFuncaoInicial && f.FuncaoId <= (int)eFuncaoFinal).ToList();
                return funcoes;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
