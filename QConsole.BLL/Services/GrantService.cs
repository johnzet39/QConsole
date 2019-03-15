using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Repositories;
using AutoMapper;

namespace QConsole.BLL.Services
{
    public class GrantService : IGrantService
    {
        GrantRepository _grantRepository;

        public GrantService(string conn)
        {
            _grantRepository = new GrantRepository(conn);
        }



        public List<UserDTO> GetGroups()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_grantRepository.GetGroups());
        }

        public List<GrantDTO> GetDicts(string grantee)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Grant, GrantDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Grant>, List<GrantDTO>>(_grantRepository.GetDicts(grantee));
        }

        public List<GrantDTO> GetLayers(string grantee)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Grant, GrantDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Grant>, List<GrantDTO>>(_grantRepository.GetLayers(grantee));
        }

        public List<UserDTO> GetUsers(string grosysid)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_grantRepository.GetUsers(grosysid));
        }

        public List<string> GrantTableToRole(string table_schema, string table_name, string role, List<string> grants_list)
        {
            return _grantRepository.GrantTableToRole(table_schema, table_name, role, grants_list);
        }
    }
}
