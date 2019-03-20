using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.BLL.Interfaces
{
    public interface IQueryService
    {
        DataTable ExecuteQuery(string queryString);
    }
}
