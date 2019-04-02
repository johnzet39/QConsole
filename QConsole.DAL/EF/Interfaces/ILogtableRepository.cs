using QConsole.DAL.EF.EDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.EF.Interfaces
{
    public interface ILogtableRepository
    {
        IEnumerable<logtable> GetAllOrdered();
    }
}
