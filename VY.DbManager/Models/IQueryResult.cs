using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VY.DbManager.Models
{
    public interface IQueryResult<T>
    {
        T Result { get; set; }
        Exception Exception { get; set; }
    }
}
