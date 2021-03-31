using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Seguranca.Data.ADO
{
    public sealed class DataAccessADO : ConexaoADO
    {
        #region Singleton Instancia Unica
        private static Dictionary<string, DataAccessADO> ListaDataAccess { get; set; }
        public static DataAccessADO Instance(IConfiguration config) { return Instance(config, "BD_Default"); }

        public static DataAccessADO Instance(IConfiguration config, string bdKey)
        {
            if (ListaDataAccess == null)
                ListaDataAccess = new Dictionary<string, DataAccessADO>();

            if (!ListaDataAccess.ContainsKey(bdKey))
                ListaDataAccess.Add(bdKey, new DataAccessADO(config, bdKey));

            return ListaDataAccess[bdKey];
        }
        #endregion

        private DataAccessADO(IConfiguration config) : base(config, "BD_Default")
        {
        }
        private DataAccessADO(IConfiguration config, string bdKey) : base(config, bdKey)
        {
        }
    }
}
