using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.DAL.EF.EDM;
using QConsole.DAL.EF.Interfaces;
using QConsole.DAL.EF.Repositories;

namespace QConsole.DAL.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MY_BASEEntities _dbContext;

        #region Repositories
        //public IRepository<logtable> LogtableRepository => 
        //    new GenericRepository<logtable>(_dbContext);
        public ILogtableRepository LogtableRepository =>
            new LogtableRepository(_dbContext);
        #endregion

        public UnitOfWork(string conn)
        {
            _dbContext = new MY_BASEEntities(conn);
        }

        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
