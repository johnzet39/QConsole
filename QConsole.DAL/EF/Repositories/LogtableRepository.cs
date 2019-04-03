using QConsole.DAL.EF.EDM;
using QConsole.DAL.EF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.EF.Repositories
{
    public class LogtableRepository : IRepository<logtable>
    {
        BaseEntities _context;
        //DbSet<logtable> _dbSet;

        public LogtableRepository(DbContext _dbContext)
        {
            _context = _dbContext as BaseEntities;
        }

        public IEnumerable<logtable> GetAll()
        {
            return _context.logtable.ToList();
        }

        public IEnumerable<logtable> GetAllOrdered()
        {
            var logrows = _context.logtable.AsNoTracking().OrderByDescending(r => r.gid);
            return logrows.ToList();
        }

        public int GetCountByOperation(string operation, int period)
        {
            DateTime date = DateTime.Now.AddDays(-period);
            var count = _context.logtable.AsNoTracking()
                .Where(o => o.action.ToUpper() == operation.ToUpper())
                .Where(o => o.timechange > date)
                .Count();
            return count;
        }

        public int GetCountInserts(string schema, string layer, int period)
        {
            int count = 0;
            DateTime date = DateTime.Now.AddDays(-period).Date;
            count = _context.logtable.AsNoTracking()
                .Where(o => o.tablename == layer)
                .Where(o => o.tableschema == schema)
                .Where(o => o.action.ToUpper() == "INSERT")
                .Where(o => o.timechange >= date)
                .Count();
            return count;
        }

        public int GetCountInsertsMonth(string schema, string layer, int month, int year)
        {
            int count = 0;
            count = _context.logtable.AsNoTracking()
                .Where(o => o.tablename == layer)
                .Where(o => o.tableschema == schema)
                .Where(o => o.action.ToUpper() == "INSERT")
                .Where(o => o.timechange.Year == year && o.timechange.Month == month)
                .Count();
            return count;
        }
    }
}
