using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.DAL.EF.Repositories;
using QConsole.DAL.EF.EDM;
using QConsole.DAL.EF.Interfaces;

namespace QConsole.DAL.EF.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<logtable> LogtableRepository { get; }
        //LogtableRepository LogtableRepository { get; }
        /// 
        /// Commits all changes
        /// 
        void Save();
        /// 
        /// Discards all changes that has not been commited
        /// 
        void RejectChanges();
        void Dispose();
    }
}
