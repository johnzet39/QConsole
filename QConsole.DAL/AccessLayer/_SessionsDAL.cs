//using System;
using System.Collections.Generic;
using System;
using System.Data;
using Npgsql;
using QgisBaseDAL.Interfaces;

namespace QgisBaseDAL.AccessLayer
{
    public class Session : ISession
    {
        public string Pid { get; set; }
        public string Application_name { get; set; }
        public DateTime Starttime { get; set; }
        public string Usename { get; set; }
        public string Descript { get; set; }
        public string Client_addr { get; set; }
        public DateTime Now { get; set; }
    }

    public class SessionsDAL : ISessionsDAL<Session>
    {
        public NpgsqlConnection _sqlConnection { get; private set; }
        public string _connectionString { get; private set; }

        public SessionsDAL(string connstring)
        {
            _connectionString = connstring;
            _sqlConnection = new NpgsqlConnection(_connectionString);
        }

        public IEnumerable<Session> GetSessionsList()
        {
            var listOfSessions = new List<Session>();
            string sql = "select \"pid\", \"application_name\", to_char(\"backend_start\",'DD.MM.YYYY HH24:MI:SS') as starttime, " +
                                " \"usename\", (select shobj_description(\"usesysid\", 'pg_authid')) as descript, \"client_addr\", " +
                                " to_char(now(),'DD.MM.YYYY HH24:MI:SS') as now from pg_stat_activity order by usename; ";

            using (_sqlConnection)
            {
                _sqlConnection.Open();
                using (var command = new NpgsqlCommand(sql, _sqlConnection))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Session session = new Session
                            {
                                Pid = dataReader["pid"].ToString(),
                                Application_name = dataReader["application_name"].ToString(),
                                Starttime = DateTime.ParseExact(dataReader["starttime"].ToString(), "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture),
                                Usename = dataReader["usename"].ToString(),
                                Descript = dataReader["descript"].ToString(),
                                Client_addr = dataReader["client_addr"].ToString(),
                                Now = DateTime.ParseExact(dataReader["now"].ToString(), "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                            };

                            listOfSessions.Add(session);
                        }
                    }
                }
            }
            return listOfSessions;
        }



        //public List<T> GetSessionsList<T>() where T : new()
        //{
        //    List<T> listOfSessions;
        //    string sql = "select \"pid\", \"application_name\", to_char(\"backend_start\",'DD.MM.YYYY HH24:MI:SS') as starttime, " +
        //                        " \"usename\", (select shobj_description(\"usesysid\", 'pg_authid')) as descript, \"client_addr\", " +
        //                        " to_char(now(),'DD.MM.YYYY HH24:MI:SS') as now from pg_stat_activity order by usename; ";

        //    using (_sqlConnection)
        //    {
        //        _sqlConnection.Open();
        //        using (var command = new NpgsqlCommand(sql, _sqlConnection))
        //        {
        //            using (var dataReader = command.ExecuteReader())
        //            {
        //                listOfSessions = MapData<T>(dataReader);

        //            }
        //        }
        //    }
        //    return listOfSessions;
        //}

        //public static List<T> MapData<T>(IDataReader dr) where T : new()
        //{
        //    Type businessEntityType = typeof(T);
        //    List<T> entitys = new List<T>();
        //    Hashtable hashtable = new Hashtable();
        //    PropertyInfo[] properties = businessEntityType.GetProperties();
        //    foreach (PropertyInfo info in properties)
        //    {
        //        hashtable[info.Name.ToUpper()] = info;
        //    }
        //    while (dr.Read())
        //    {
        //        T newObject = new T();
        //        for (int index = 0; index < dr.FieldCount; index++)
        //        {
        //            PropertyInfo info = (PropertyInfo)
        //                                hashtable[dr.GetName(index).ToUpper()];
        //            if ((info != null) && info.CanWrite)
        //            {
        //                info.SetValue(newObject, dr.GetValue(index), null);
        //            }
        //        }
        //        entitys.Add(newObject);
        //    }
        //    dr.Close();
        //    return entitys;
        //}

    }
}
