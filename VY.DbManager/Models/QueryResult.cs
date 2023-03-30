using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VY.DbManager.Models
{
    public class QueryResult<T> : IQueryResult<T>
    {
        public T Result { get; set; }
        public Exception Exception { get; set; }
    }
}
