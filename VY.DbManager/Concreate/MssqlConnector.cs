using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VY.DbManager.Enums;
using VY.DbManager.Models;
using VY.DBManager.Abstract;

namespace VY.DBManager.Concreate
{
    internal class MssqlConnector : IConnectors
    {
        private ConnectionInformation _connectionInformations;
        public DatabaseType DbType => DatabaseType.Mssql;

        public MssqlConnector(ConnectionInformation connectionInformations)
        {
            _connectionInformations = connectionInformations;
        }

        public bool ExecuteQuery(string query, out Exception ex)
        {
            bool result = true;
            ex = null;

            try
            {
                using (var conn = new SqlConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new SqlCommand(query, conn))
                    {
                        result = command.ExecuteNonQuery() > 0;
                    }

                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                result = false;
                ex = exception;
            }

            return result;
        }

        public bool ExecuteQuery(string query, int commandTimeOut, out Exception ex)
        {
            bool result = true;
            ex = null;

            try
            {
                using (var conn = new SqlConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new SqlCommand(query, conn))
                    {
                        command.CommandTimeout = commandTimeOut;
                        result = command.ExecuteNonQuery() > 0;
                    }

                    conn.Close();
                }
            }
            catch (Exception exception)
            {
                result = false;
                ex = exception;
            }

            return result;
        }

        public DataTable GetTable(string query, out Exception ex)
        {
            DataTable dt = new DataTable();
            ex = null;

            try
            {
                using (var conn = new SqlConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(command);

                        da.Fill(dt);
                    }

                    conn.Close();
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
                using (var conn = new SqlConnection(_connectionInformations.ConnectionString))
                {
                    conn.Open();

                    using (var command = new SqlCommand(query, conn))
                    {
                        command.CommandTimeout = commandTimeOut;

                        SqlDataAdapter da = new SqlDataAdapter(command);

                        da.Fill(dt);
                    }

                    conn.Close();
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
            ex = null;

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
                ex = exception;
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

                if (dt != null)
                    result = dt.Rows.Count > 0;
            }
            catch (Exception exception)
            {
                result = false;
                ex = exception;
            }

            return result;
        }

        public string GetJson(string query)
        {
            string result = string.Empty;

            DataTable dt = new DataTable();
            using (var connection = new SqlConnection(_connectionInformations.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(command);

                    da.Fill(dt);
                }

                connection.Close();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                result = JsonConvert.SerializeObject(dt);
            }

            return result;
        }


    }
}
