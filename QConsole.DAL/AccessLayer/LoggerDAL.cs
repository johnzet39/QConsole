﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace QConsole.DAL.AccessLayer
{
    public class LogRow
    {
        public string Gid{ get; set; }
        public string Action{ get; set; }
        public string Username{ get; set; }
        public string Address{ get; set; }
        public DateTime Datechange { get; set; }
        public string Tablename{ get; set; }
        public string Gidnum{ get; set; }
        public string Context{ get; set; }
    }

    public class LoggerDAL
    {
        private string _connectionString;

        public LoggerDAL(string connstring)
        {
            _connectionString = connstring;
        }

        //LOG LIST main
        public List<LogRow> GetLogList(string ExtraQueryFull, string FirstRowsQuery)
        {
            string sql_query = String.Format("SELECT \"gid\", \"action\", \"username\", \"address\", to_char(\"timechange\",'DD.MM.YYYY HH24:MI:SS') as \"datechange\", " +
                                " \"tablename\", \"gidnum\", \"context\"   FROM logger.logtable {0}  order by timechange DESC {1} ", ExtraQueryFull, FirstRowsQuery);
            return GetListOfObjects(sql_query);

        }

        //get list of Groups/Users
        private List<LogRow> GetListOfObjects(string sql_query)
        {
            var listOfObjects = new List<LogRow>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                    conn.Open();
                    using (var command = new NpgsqlCommand(sql_query, conn))
                    {
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objectpsql = new LogRow();

                                objectpsql.Gid = dataReader["Gid"].ToString();
                                objectpsql.Action = dataReader["Action"].ToString();
                                objectpsql.Username = dataReader["Username"].ToString();
                                objectpsql.Address = dataReader["Address"].ToString();
                                objectpsql.Datechange = DateTime.ParseExact(dataReader["Datechange"].ToString(), "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                objectpsql.Tablename = dataReader["Tablename"].ToString();
                                objectpsql.Gidnum = dataReader["Gidnum"].ToString();
                                objectpsql.Context = dataReader["Context"].ToString();

                                listOfObjects.Add(objectpsql);
                            }
                        }
                    }

            }
            return listOfObjects;
        }


        //build date string subquery
        public string BuildExtraDateString(DateTime? dateFrom, DateTime? dateTo)
        {
            List<string> list = new List<string>();
            if (dateFrom.HasValue)
            {
                string dateFrom_formated = dateFrom.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string DateFromString = string.Format("\"timechange\" >= to_date('{0}', 'DD.MM.YYYY')", dateFrom_formated);
                list.Add(DateFromString);
            }
            if (dateTo.HasValue)
            {
                string dateTo_formated = ((DateTime)dateTo).AddDays(1).ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string DateToString = string.Format("\"timechange\" < to_date('{0}', 'DD.MM.YYYY')", dateTo_formated);
                list.Add(DateToString);
            }
            string ExtraDateString = string.Join(" AND ", list);
            return ExtraDateString;
        }
        //build 1000rows string subquery
        public string BuildExtraFirstRowsString()
        {
            return " limit 1000";
        }
        //union extra subquery strings
        public string UnionExtraStrings(IList<string> str)
        {
            string extraQueryFull = string.Join(" AND ", str);
            return " WHERE " + extraQueryFull;
        }
        //get column list for combobox
        public List<string> GetColumnsList()
        {
            return new List<string>() { "gid", "action", "username", "address", "tablename", "gidnum", "context" };
        }
    }
}