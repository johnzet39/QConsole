using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using AutoMapper;
using QConsole.DAL.AccessLayer.Manager;

namespace QConsole.BLL.Services
{
    public class UserService : IUserService
    {
        IManagerDAL _managerDAL;

        public UserService(string conn)
        {
            _managerDAL = new ManagerDAL(conn);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_managerDAL.UserAccess.GetUsers());
        }

        public DataTable GetAssignedRoles(string oid)
        {
            return _managerDAL.UserAccess.GetAssignedRoles(oid);
        }

        public DataTable GetAvailableRoles(string oid)
        {
            return _managerDAL.UserAccess.GetAvailableRoles(oid);
        }

        public string GrantRole(string userName, string roleName)
        {
            return _managerDAL.UserAccess.GrantRole(userName, roleName);
        }

        public string RevokeRole(string userName, string roleName)
        {
            return _managerDAL.UserAccess.RevokeRole(userName, roleName);
        }

        public string RemoveRoleOrUser(string userName)
        {
            return _managerDAL.UserAccess.RemoveRoleOrUser(userName);
        }

        public IEnumerable<string> CreateUserOrRole(string userName, string passWord, string definition)
        {
            return _managerDAL.UserAccess.CreateUserOrRole(userName, passWord, definition);
        }

        public IEnumerable<string> EditUserOrRole(string userName, string passWord, string definition)
        {
            return _managerDAL.UserAccess.EditUserOrRole(userName, passWord, definition);
        }
    }
}
