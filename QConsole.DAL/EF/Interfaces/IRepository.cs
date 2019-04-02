using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.DAL.EF.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

    }
}
