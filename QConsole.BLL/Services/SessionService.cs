using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.Interfaces;
using QConsole.BLL.DTO;
using QConsole.DAL.AccessLayer.Entities;
using AutoMapper;
using QConsole.DAL.AccessLayer.Manager;

namespace QConsole.BLL.Services
{
    public class SessionService : ISessionService
    {
        IManagerDAL _managerDAL;

        public SessionService(string conn)
        {
            _managerDAL = new ManagerDAL(conn);
        }

        public IEnumerable<SessionDTO> GetSessions()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Session, SessionDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Session>, List<SessionDTO>>(_managerDAL.SessionAccess.GetSessionsList());
        }
    }
}
