using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VY.DbManager.Enums;
using VY.DbManager.Models;
using VY.DBManager.Abstract;

namespace VY.DBManager.Concreate
{
    internal class FirebirdConnector : IConnectors
    {
        private ConnectionInformation _connectionInformations;
        public DatabaseType DbType => DatabaseType.Firebird;

        public FirebirdConnector(ConnectionInformation connectionInformations)
        {
            _connectionInformations = connectionInformations;
        }

        public bool ExecuteQuery(string query, out Exception ex)
        {
            bool result = false;
            ex          = null;

            try
            {
                using (var conn = new FbConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new FbCommand(query, conn))
                    {
                        result = command.ExecuteNonQuery() > 0;
                    }

                    conn.Close();

                    FbConnection.ClearAllPools();
                }
            }
            catch (Exception exception)
            {
                result = false;
                ex     = exception;
            }

            return result;
        }

        public bool ExecuteQuery(string query, int commandTimeOut, out Exception ex)
        {
            bool result = false;
            ex = null;

            try
            {
                using (var conn = new FbConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new FbCommand(query, conn))
                    {
                        command.CommandTimeout = commandTimeOut;
                        result = command.ExecuteNonQuery() > 0;
                    }

                    conn.Close();

                    FbConnection.ClearAllPools();
                }
            }
            catch (Exception exception)
            {
                result = false;
                ex = exception;
            }

            return false;
        }

        public DataTable GetTable(string query, out Exception ex)
        {
            DataTable dt = new DataTable();
            ex = null;

            try
            {
                using (var conn = new FbConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new FbCommand(query, conn))
                    {
                        FbDataAdapter da = new FbDataAdapter(command);

                        da.Fill(dt);
                    }

                    conn.Close();

                    FbConnection.ClearAllPools();
                }
            } 
            catch (Exception exception)
            {
                dt = new DataTable();
                ex = exception;
            }

            return dt;
        }

        public DataTable GetTable(string query, int commandTimeOut, out Exception ex)
        {
            DataTable dt = new DataTable();
            ex = null;

            try
            {
                using (var conn = new FbConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new FbCommand(query, conn))
                    {
                        command.CommandTimeout = commandTimeOut;

                        FbDataAdapter da = new FbDataAdapter(command);

                        da.Fill(dt);
                    }

                    conn.Close();

                    FbConnection.ClearAllPools();
                }
            }
            catch (Exception exception)
            {
                dt = new DataTable();
                ex = exception;
            }

            return dt;
        }


        public List<string> GetColumnList(string query, out Exception ex)
        {
            List<string> columnList = new List<string>();

            try
            {
                DataTable dt = GetTable(query, out ex);

                columnList = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToList();
            }
            catch (Exception exception)
            {
                columnList = new List<string>();
                ex         = exception;
            }

            return columnList;
        }

        /// <summary>
        /// Veri tabanı bağlantısını test eder.
        /// </summary>
        /// <returns></returns>
        public bool TestConnection(out Exception ex)
        {
            bool result = false;

            ex = null;

            try
            {
                DataTable dt = GetTable(_connectionInformations.ConnectionTestQuery, out ex);

                if(ex == null) 
                        result = dt.Rows.Count > 0;
            }
            catch 
            {
                result = false;
            }

            return result;
        }
        public string GetJson(string query)
        {
            string result = string.Empty;

            DataTable dt = new DataTable();
            using (var connection = new FbConnection(_connectionInformations.ConnectionString))
            {
                connection.Open();

                using (var command = new FbCommand(query, connection))
                {
                    FbDataAdapter da = new FbDataAdapter(command);

                    da.Fill(dt);
                }

                connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                result = JsonConvert.SerializeObject(dt);
            }

            return result;
        }
    }
}
