using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Models.Tabs
{
    class ConfigsTab : Tab
    {
        public ConfigsTab()
        {
            Name = "Conf";
            IconPath = "../../../Icons/_tabControl/ConfigurationEditor_16x.png";
            IsClosable = true;
        }
    }
}
