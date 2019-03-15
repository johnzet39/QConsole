using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.Models;

namespace QConsole.Models.Tabs
{
    public class UserTab : Tab
    {
        public UserTab()
        {
            Name = "Пользователи";
            IconPath = "../../../Icons/_tabControl/User_16x.png";
        }
    }
}
