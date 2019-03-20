using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Interfaces;
using QConsole.DAL.AccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.BLL.Services
{
    public class QueryService : IQueryService
    {
        IQueryRepository _queryRepository;
        public QueryService(string conn)
        {
            _queryRepository = new QueryRepository(conn);
        }

        public DataTable ExecuteQuery(string queryString)
        {
            return _queryRepository.ExecuteQuery(queryString);
        }
    }
}
