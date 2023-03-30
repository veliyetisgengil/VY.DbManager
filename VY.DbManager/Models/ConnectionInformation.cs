using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VY.DbManager.Enums;

namespace VY.DbManager.Models
{
    public class ConnectionInformation
    {
        private string _connectionString;
        private string _connectionTestQuery;

        public DatabaseType DbType { get; set; } = DatabaseType.None;
        public string DataSource { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public string Port { get; set; } = "3050";
        public string Charset { get; set; } = "NONE";
        public int TimeOut { get; set; } = 30;
        public string ConnectionString { get { return GetConnectionString(); } set { _connectionString = value; } }
        public string ConnectionTestQuery { get { return GetConnectionTestQuery(); } set { _connectionTestQuery = value; } }


        private string GetConnectionString()
        {
            string connectionString = string.Empty;

            if (!string.IsNullOrEmpty(DataSource))
            {
                switch (DbType)
                {
                    case DatabaseType.None:
                        break;
                    case DatabaseType.Firebird:
                        if (GetIsIpAddress())
                            connectionString = $@"character set={Charset};data source={DataSource};initial catalog={Database};user id={UserID};password={Password};";
                        else
                            connectionString = $@"User ID={UserID};Password={Password};Database={Database};DataSource={DataSource};Charset={Charset};"; //$@"User ID={UserID};Password={Password};Database=localhost:{DataSource};Charset={Charset};READONLY={PermissionTypeTextToFirebird};";                                                                                                                     
                        break;
                    case DatabaseType.Mssql:
                        connectionString = $"Data Source={DataSource};Initial Catalog={Database};Integrated Security = False;User ID={UserID}; Password={Password};Encrypt = False; TrustServerCertificate = True; MultiSubnetFailover = False; Connect Timeout={TimeOut.ToString()}";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                connectionString = _connectionString;
            }



            return connectionString;
        }

        private string GetConnectionTestQuery()
        {
            string connectionTestQuery = string.Empty;

            if (!string.IsNullOrEmpty(DataSource))
            {
                switch (DbType)
                {
                    case DatabaseType.None:
                        break;
                    case DatabaseType.Firebird:
                        connectionTestQuery = "select cast('Now' as date) from rdb$database;";
                        break;
                    case DatabaseType.Mssql:
                        connectionTestQuery = "select getdate();";
                        break;
                
                    default:
                        break;
                }
            }
            else
            {
                connectionTestQuery = _connectionTestQuery;
            }

            return connectionTestQuery;
        }

        private bool GetIsIpAddress()
        {
            bool result = false;

            if (!string.IsNullOrEmpty(DataSource))
            {
                if (DataSource.Length > 0)
                {
                    result = DataSource.Length - DataSource.Replace(".", "").Length == 4;
                }
            }

            return result;
        }

    }
}
