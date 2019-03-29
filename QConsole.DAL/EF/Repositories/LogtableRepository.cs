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
    class LogtableRepository : ILogtableRepository
    {
        MY_BASEEntities _context;
        DbSet<logtable> _dbSet;

        public LogtableRepository(DbContext _dbContext)
        {
            _context = _dbContext as MY_BASEEntities;
        }

        public IEnumerable<logtable> GetAllOrdered()
        {
            var logrows = _context.logtable.AsNoTracking().OrderByDescending(r => r.timechange);
            return logrows.ToList();
        }
    }
}
