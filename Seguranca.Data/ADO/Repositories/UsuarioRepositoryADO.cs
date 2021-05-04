using Microsoft.Extensions.Configuration;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Seguranca.Data.ADO.Repositories
{
    public class UsuarioRepositoryADO : IUsuarioRepository
    {
        private readonly DataAccessADO _context;
        public UsuarioRepositoryADO(IConfiguration config)
        {
            _context = DataAccessADO.Instance(config, "SegurancaConnection");            
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        #region ObterAsync
        public async Task<IEnumerable<Usuario>> ObterAsync()
        {
            var query = @"SELECT 
	                        u.UsuarioId, u.Nome, u.Email, u.Senha FROM Usuario u";
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

        public async Task<Usuario> ObterAsync(int id)
        {
            var query = $@"SELECT 
	                        u.UsuarioId, u.Nome, u.Email, u.Senha FROM Usuario u
                         WHERE UsuarioId = '{id}'";
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

        public async Task<Usuario> ObterAsync(string email)
        {
            var query = $@"SELECT 
	                        u.UsuarioId, u.Nome, u.Email, u.Senha FROM Usuario u
                         WHERE Email = '{email}'";
            try
            {
                _context.AbrirConexao();
                _context.DbCommand.CommandType = System.Data.CommandType.Text;
                _context.DbCommand.CommandText = query;

                return ObterLista(await _context.DbCommand.ExecuteReaderAsync()).First();
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
        public Task<IEnumerable<Usuario>> ObterAsyncFull()
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ObterAsyncFull(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Insere
        public void Insere(Usuario entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public void Update(Usuario entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public void Remove(Usuario entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        private List<Usuario> ObterLista(DbDataReader dr)
        {
            var usuarios = new List<Usuario>();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    usuarios.Add(new Usuario()
                    {
                        UsuarioId = (int)dr["UsuarioId"],
                        Nome = dr["Nome"].ToString(),
                        Email = dr["Email"].ToString(),
                    });
                }
            }
            return usuarios;
        }
    }
}
