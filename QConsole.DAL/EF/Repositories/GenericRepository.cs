using QConsole.DAL.EF.EDM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.EF.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        DbContext _context;
        DbSet<T> _dbSet;


        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<T> GetAllOrderByTimeChange()
        {
            return _dbSet.AsNoTracking().ToList();
        }
    }
}
