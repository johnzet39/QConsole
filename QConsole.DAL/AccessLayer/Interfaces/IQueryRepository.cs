using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.AccessLayer.Interfaces
{
    public interface IQueryRepository
    {
        DataTable ExecuteQuery(string queryString);
    }
}
