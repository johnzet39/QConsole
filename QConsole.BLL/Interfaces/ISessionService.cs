using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;

namespace QConsole.BLL.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionDTO> GetSessions();
    }
}
