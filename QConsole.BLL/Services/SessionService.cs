﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.Interfaces;
using QConsole.BLL.DTO;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Repositories;
using AutoMapper;

namespace QConsole.BLL.Services
{
    public class SessionService : ISessionService
    {
        SessionRepository _sessionRepository;

        public SessionService(string conn)
        {
            _sessionRepository = new SessionRepository(conn);
        }

        public IEnumerable<SessionDTO> GetSessions()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Session, SessionDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Session>, List<SessionDTO>>(_sessionRepository.GetSessionsList());
        }
    }
}