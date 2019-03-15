using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QConsole.BLL;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace QConsole.Common
{
    static class ConnectionStrings
    {
        //file with connectionStrings
        static public string _connectionStringsFilePath { get; set; }
        // Current Connection string 
        static public string ConnectionString { get; set; }

        //deserialize list of connection from user settings
        static public List<ConnectionBase> LoadConnectionStrings()
        {
            #region load from user.config
            //using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.ConnStrings)))
            //{
            //    BinaryFormatter bf = new BinaryFormatter();
            //    return (List<ConnectionBase>)bf.Deserialize(ms);
            //}
            #endregion
            List<ConnectionBase> list = new List<ConnectionBase>();
            if (File.Exists(_connectionStringsFilePath))
            {
                CollectionSettings collection;
                var xmlSerializer = new XmlSerializer(typeof(CollectionSettings));
                using (XmlReader reader = XmlReader.Create(_connectionStringsFilePath))
                {
                    collection = (CollectionSettings)xmlSerializer.Deserialize(reader);
                    list = collection.Collection;
                }
            }
            return (list);
        }

        //serialize list of connection in user settings
        static public void SaveConnectionStrings(List<ConnectionBase> connectionBases)
        {
            #region save to user.config comment
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    BinaryFormatter bf = new BinaryFormatter();
            //    bf.Serialize(ms, connectionBases);
            //    ms.Position = 0;
            //    byte[] buffer = new byte[(int)ms.Length];
            //    ms.Read(buffer, 0, buffer.Length);
            //    Properties.Settings.Default.ConnStrings = Convert.ToBase64String(buffer);
            //    Properties.Settings.Default.Save();
            //}
            #endregion
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CollectionSettings));
            CollectionSettings cbc = new CollectionSettings
            {
                Collection = connectionBases
            };
            using (FileStream fs = new FileStream(_connectionStringsFilePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, cbc);
            }
        }
    }

}
