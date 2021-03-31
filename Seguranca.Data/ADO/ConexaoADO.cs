using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace Seguranca.Data.ADO
{
    public abstract class ConexaoADO
    {
        protected string ConnectionString { get; private set; } 
        protected string ProviderName { get; private set; }
        protected DbConnection DbConnection { get; private set; }
        protected DbTransaction DbTransaction { get; private set; }
        public DbCommand DbCommand { get; private set; }

        protected ConexaoADO(IConfiguration config, string bdKey)
        {
            ProviderName = "System.Data.SqlClient";
            ConnectionString = config.GetConnectionString(bdKey);
            DbConnection = null;
            DbTransaction = null;
            AbrirConexao();
            FecharConexao();
        }

        #region AbrirConexao
        public void AbrirConexao()
        {
            if (this.DbConnection == null)
            {
                DbProviderFactories.RegisterFactory(
                    ProviderName, Microsoft.Data.SqlClient.SqlClientFactory.Instance);

                var factory = DbProviderFactories.GetFactory(this.ProviderName);
                DbConnection = factory.CreateConnection();
                this.DbConnection.ConnectionString = this.ConnectionString;
            }

            if (this.DbConnection.State == ConnectionState.Closed)
            {
                this.DbConnection.Open();
            }
            DbCommand = this.DbConnection.CreateCommand();
            this.DbCommand.Connection = this.DbConnection;
            this.DbCommand.Transaction = this.DbTransaction;
        }
        #endregion

        #region FecharConexao
        public void FecharConexao()
        {
            if (this.DbTransaction == null)
            {
                this.DbConnection.Close();
            }
        }
        #endregion

        #region BeginTransaction
        /// <summary>
        /// Iniciar Transação
        /// </summary>
        public void BeginTransaction()
        {
            if (this.DbTransaction == null)
            {
                if (this.DbConnection == null)
                {
                    var factory = DbProviderFactories.GetFactory(this.ProviderName);
                    DbConnection = factory.CreateConnection();
                    this.DbConnection.ConnectionString = this.ConnectionString;
                }
                this.DbConnection.Open();
                DbTransaction = this.DbConnection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            AbrirConexao();
        }
        #endregion

        #region Commit
        /// <summary>
        /// Finalizar Transação
        /// </summary>
        public void Commit()
        {
            if (this.DbConnection != null)
            {
                this.DbTransaction.Commit();
                this.DbConnection.Close();
                DbTransaction = null;
            }
        }
        #endregion

        #region Rollback
        /// <summary>
        /// Abortar Transação
        /// </summary>
        public void Rollback()
        {
            if (this.DbConnection != null)
            {
                this.DbTransaction.Rollback();
                this.DbConnection.Close();
                DbTransaction = null;
            }
        }
        #endregion
    }
}
