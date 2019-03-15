using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.DAL.AccessLayer.Entities;

namespace QConsole.DAL.AccessLayer.Interfaces
{
    public interface IUserRepository
    {
        //all users
        IEnumerable<User> GetUsers();

        //assigned roles for user
        DataTable GetAssignedRoles(string oid);

        //available roles for user
        DataTable GetAvailableRoles(string oid);

        //grant available role
        string GrantRole(string userName, string roleName);

        //revoke assigned role
        string RevokeRole(string userName, string roleName);

        //remove role/user
        string RemoveRoleOrUser(string userName);

        //create new role/user
        IEnumerable<String> CreateUserOrRole(string userName, string passWord, string definition);

        //edit role/user
        IEnumerable<String> EditUserOrRole(string userName, string passWord, string definition);
    }
}
