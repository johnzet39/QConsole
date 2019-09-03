﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Manager;
using AutoMapper;

namespace QConsole.BLL.Services
{
    public class GrantService : IGrantService
    {
        IManagerDAL _managerDAL;

        public GrantService(string conn)
        {
            _managerDAL = new ManagerDAL(conn);
        }


        public List<UserDTO> GetGroups()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_managerDAL.GrantAccess.GetGroups());
        }

        public List<GrantDTO> GetDicts(string grantee)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Grant, GrantDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Grant>, List<GrantDTO>>(_managerDAL.GrantAccess.GetDicts(grantee));
        }

        public List<GrantDTO> GetLayers(string grantee)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Grant, GrantDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Grant>, List<GrantDTO>>(_managerDAL.GrantAccess.GetLayers(grantee));
        }

        public List<UserDTO> GetUsers(string grosysid)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_managerDAL.GrantAccess.GetUsers(grosysid));
        }

        public List<string> GrantTableToRole(string table_schema, string table_name, string role, List<string> grants_list)
        {
            return _managerDAL.GrantAccess.GrantTableToRole(table_schema, table_name, role, grants_list);
        }
    }
}
