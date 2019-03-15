using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;

namespace QConsole.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetUsers();
        DataTable GetAssignedRoles(string oid);
        DataTable GetAvailableRoles(string oid);
        string GrantRole(string userName, string roleName);
        string RevokeRole(string userName, string roleName);
        string RemoveRoleOrUser(string userName);
        IEnumerable<String> CreateUserOrRole(string userName, string passWord, string definition);
        IEnumerable<String> EditUserOrRole(string userName, string passWord, string definition);
    }
}
