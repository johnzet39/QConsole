﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL.DTO;

namespace QConsole.BLL.Interfaces
{
    public interface IGrantService
    {
        //groups
        List<UserDTO> GetGroups();
        //users in selected group
        List<UserDTO> GetUsers(string grosysid);
        //layers list for selected grantee (role)
        List<GrantDTO> GetLayers(string grantee);
        //dicts list for selected grantee (role)
        List<GrantDTO> GetDicts(string grantee);
        //Grant privileges to selected role
        List<string> GrantTableToRole(string table_schema, string table_name, string role, List<string> grants_list);
    }
}