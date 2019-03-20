using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Models.Tabs
{
    class QueryTab : Tab
    {
        public QueryTab()
        {
            Name = "SQL";
            IconPath = "../../../Icons/_tabControl/QueryView_16x.png";
            IsClosable = true;
        }
    }
}
