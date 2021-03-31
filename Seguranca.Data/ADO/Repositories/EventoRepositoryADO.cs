using Microsoft.Extensions.Configuration;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Data.ADO.Repositories
{
    public class EventoRepositoryADO : IEventoRepository
    {
        private readonly DataAccessADO _context;
        public EventoRepositoryADO(IConfiguration config)
        {
            _context = DataAccessADO.Instance(config, "SegurancaConnection");
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        #region ObterAsync
        public async Task<IEnumerable<Evento>> ObterAsync()
        {
            var query = @"SELECT 
	                        e.EventoId, e.Nome, e.Descricao FROM Evento e";
            try
            {
                _context.AbrirConexao();
                _context.DbCommand.CommandType = System.Data.CommandType.Text;
                _context.DbCommand.CommandText = query;

                var dr = await _context.DbCommand.ExecuteReaderAsync();
                return ObterLista(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                _context.FecharConexao();
            }
        }

        public async Task<Evento> ObterAsync(int id)
        {
            var query = $@"SELECT 
	                        e.EventoId, e.Nome, e.Descricao FROM Evento e
                         WHERE EventoId = '{id}'";
            try
            {
                _context.AbrirConexao();
                _context.DbCommand.CommandType = System.Data.CommandType.Text;
                _context.DbCommand.CommandText = query;

                var dr = await _context.DbCommand.ExecuteReaderAsync();
                return ObterLista(dr).First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                _context.FecharConexao();
            }
        }
        #endregion

        #region ObterAsyncFull
        public Task<IEnumerable<Evento>> ObterAsyncFull()
        {
            throw new NotImplementedException();
        }

        public Task<Evento> ObterAsyncFull(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Insere
        public void Insere(Evento entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public void Update(Evento entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public void Remove(Evento entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        private List<Evento> ObterLista(DbDataReader dr)
        {
            var eventos = new List<Evento>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    eventos.Add(new Evento()
                    {
                        EventoId = (int)dr["EventoId"],
                        Nome = dr["Nome"].ToString(),
                        Descricao = dr["Descricao"].ToString(),
                    });
                }
            }
            return eventos;
        }
    }
}
