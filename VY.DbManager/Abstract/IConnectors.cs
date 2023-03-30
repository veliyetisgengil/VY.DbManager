using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VY.DbManager.Enums;

namespace VY.DBManager.Abstract
{
    internal interface IConnectors
    {
        DatabaseType DbType { get; }
        bool ExecuteQuery(string query, out Exception ex);
        bool ExecuteQuery(string query, int commandTimeOut, out Exception ex);
        DataTable GetTable(string query, out Exception ex);
        DataTable GetTable(string query, int commandTimeOut, out Exception ex);
        List<string> GetColumnList(string query, out Exception ex);
        bool TestConnection(out Exception ex);

        string GetJson(string query);


    }
}
