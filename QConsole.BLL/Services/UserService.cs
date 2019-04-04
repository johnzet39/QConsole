using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;
using QConsole.BLL.Interfaces;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.DAO;
using AutoMapper;

namespace QConsole.BLL.Services
{
    public class UserService : IUserService
    {
        UserDAO _userRepository;

        public UserService(string conn)
        {
            _userRepository = new UserDAO(conn);
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(_userRepository.GetUsers());
        }

        public DataTable GetAssignedRoles(string oid)
        {
            return _userRepository.GetAssignedRoles(oid);
        }

        public DataTable GetAvailableRoles(string oid)
        {
            return _userRepository.GetAvailableRoles(oid);
        }

        public string GrantRole(string userName, string roleName)
        {
            return _userRepository.GrantRole(userName, roleName);
        }

        public string RevokeRole(string userName, string roleName)
        {
            return _userRepository.RevokeRole(userName, roleName);
        }

        public string RemoveRoleOrUser(string userName)
        {
            return _userRepository.RemoveRoleOrUser(userName);
        }

        public IEnumerable<string> CreateUserOrRole(string userName, string passWord, string definition)
        {
            return _userRepository.CreateUserOrRole(userName, passWord, definition);
        }

        public IEnumerable<string> EditUserOrRole(string userName, string passWord, string definition)
        {
            return _userRepository.EditUserOrRole(userName, passWord, definition);
        }
    }
}
