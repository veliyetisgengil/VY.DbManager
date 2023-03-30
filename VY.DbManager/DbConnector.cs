using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VY.DbManager.Models;
using VY.DBManager.Abstract;
using VY.DBManager.Concreate;

namespace VY.DBManager
{
    public class DbConnector
    {
        private List<IConnectors> _connectors;
        private IConnectors _connector => GetConnector();

        private ConnectionInformation _connectionInformations;

        public DbConnector(ConnectionInformation connectionInformations)
        {
            _connectors = new List<IConnectors>();
            _connectionInformations = connectionInformations;

            _connectors.Add(new MssqlConnector(connectionInformations));
            _connectors.Add(new FirebirdConnector(connectionInformations));
         
        }

        private IConnectors GetConnector()
        {
            return _connectors.Where(x => x.DbType == _connectionInformations.DbType).FirstOrDefault();
        }

        public IQueryResult<bool> ExecuteQuery(string query)
        {
            QueryResult<bool> queryResult = new QueryResult<bool>();

            queryResult.Result    = _connector.ExecuteQuery(query, out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }

        public IQueryResult<bool> ExecuteQuery(string query, int commandTimeOut)
        {
            QueryResult<bool> queryResult = new QueryResult<bool>();

            queryResult.Result = _connector.ExecuteQuery(query, commandTimeOut, out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }


        /// <summary>
        /// Query sonucunu DataTable olarak dönderir.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryResult<DataTable> GetTable(string query)
        {
            QueryResult<DataTable> queryResult = new QueryResult<DataTable>();

            queryResult.Result    = _connector.GetTable(query, out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }

      

      

        




        /// <summary>
        /// Query sonucunu DataTable olarak dönderir.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryResult<DataTable> GetTable(string query, int commandTimeOut)
        {
            QueryResult<DataTable> queryResult = new QueryResult<DataTable>();

            queryResult.Result = _connector.GetTable(query, commandTimeOut, out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }

        

        

       

        

        public IQueryResult<string> GetJson(string query)
        {
            QueryResult<string> queryResult = new QueryResult<string>();

            IQueryResult<DataTable> dt = GetTable(query);

            if (dt != null)
                queryResult.Result = JsonConvert.SerializeObject(dt.Result);

            queryResult.Exception = dt.Exception;

            return queryResult;
        }
         
     
        
        /// <summary>
        /// Query sonucunun ilk kolonunu string olarak dönderir
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryResult<string> GetData(string query)
        {
            QueryResult<string> queryResult = new QueryResult<string>();

            try
            {
                DataTable dt = _connector.GetTable(query, out Exception ex);

                queryResult.Result    = dt.Rows[0][0].ToString();
                queryResult.Exception = ex;
            }
            catch (Exception ex)
            {

            }

            return queryResult;
        }



       

        public IQueryResult<string> GetJson(string query, int commandTimeOut)
        {
            QueryResult<string> queryResult = new QueryResult<string>();

            IQueryResult<DataTable> dt = GetTable(query, commandTimeOut);

            if (dt != null)
                queryResult.Result = JsonConvert.SerializeObject(dt.Result);

            queryResult.Exception = dt.Exception;

            return queryResult;
        }

     

        /// <summary>
        /// Query sonucunun ilk kolonunu string olarak dönderir
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryResult<string> GetData(string query, int commandTimeOut)
        {
            QueryResult<string> queryResult = new QueryResult<string>();

            try
            {
                DataTable dt = _connector.GetTable(query, commandTimeOut, out Exception ex);

                queryResult.Result = dt.Rows[0][0].ToString();
                queryResult.Exception = ex;
            }
            catch (Exception ex)
            {

            }

            return queryResult;
        }


        public IQueryResult<List<string>> GetColumnList(string query)
        {
            QueryResult<List<string>> queryResult = new QueryResult<List<string>>();

            queryResult.Result    = _connector.GetColumnList(query, out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }

        /// <summary>
        /// Veri tabanı bağlantısını test eder.
        /// </summary>
        /// <returns></returns>
        public IQueryResult<bool> TestConnection()
        {
            QueryResult<bool> queryResult = new QueryResult<bool>();

            queryResult.Result    = _connector.TestConnection(out Exception ex);
            queryResult.Exception = ex;

            return queryResult;
        }

    }
}
