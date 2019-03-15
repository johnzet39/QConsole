using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using QConsole.DAL.Entities;

namespace QConsole.BLL
{
    public class ConnectionBase : ConnectionBaseDAL
    {
    }

    public class CollectionSettings
    {
        [XmlArray("CollectionConnectionBase"), XmlArrayItem("Item")]
        public List<ConnectionBase> Collection { get; set; }
    }
}
