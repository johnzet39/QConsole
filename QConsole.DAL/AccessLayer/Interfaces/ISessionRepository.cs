using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.AccessLayer.Interfaces
{
    public interface ISessionRepository<T>
    {
        IEnumerable<T> GetSessionsList();
    }

}
