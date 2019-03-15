using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.BLL.DTO
{
    public class LayerDTO
    {
        public string Table_schema { get; set; }
        public string Table_name { get; set; }
        public string Descript { get; set; }
        public string Geomtype { get; set; }
        public Boolean Isupdater { get; set; }
        public Boolean Islogger { get; set; }
    }
}
