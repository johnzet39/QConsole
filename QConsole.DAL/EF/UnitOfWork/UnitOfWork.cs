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
        private readonly BaseEntities _dbContext;

        #region Repositories

        private IRepository<logtable> logtableRepository;
        public IRepository<logtable> LogtableRepository
        {
            get
            {
                if (logtableRepository == null)
                    logtableRepository = new GenericRepository<logtable>(_dbContext);
                return logtableRepository;
            }
        }
        #endregion

        public UnitOfWork(string conn)
        {
            _dbContext = new BaseEntities(conn);

            //################# DEBUGGINGGGG ###############
            //_dbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //################# DEBUGGINGGGG ###############
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
