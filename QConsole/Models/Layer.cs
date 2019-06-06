using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QConsole.Models
{
    public class Layer
    {
        public string Table_schema { get; set; }
        public string Table_name { get; set; }
        public string Descript { get; set; }
        public string Geomtype { get; set; }
        public Boolean Isupdater { get; set; }
        public Boolean Islogger { get; set; }
    }

    public class Dict
    {
        public int Id { get; set; }
        public string Schema_name { get; set; }
        public string Table_name { get; set; }
    }

    public class InformationSchemaTable
    {
        public string Table_schema { get; set; }
        public string Table_name { get; set; }
        public string Table_type { get; set; }
    }
}
