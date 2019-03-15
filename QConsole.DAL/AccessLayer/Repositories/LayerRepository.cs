﻿using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Interfaces;

namespace QConsole.DAL.AccessLayer.Repositories
{
    // queries for create/edit layers
    public static class LayerQueries
    {
        public static string CommentOnTable(string tableschema, string tablename, string definition)
        {
            return String.Format("COMMENT ON TABLE {0}.{1} IS '{2}';", tableschema, tablename, definition);
        }

        public static string SetUpdater(string tableschema, string tablename, Boolean isupdater)
        {
            return String.Format("SELECT qfunc_addupdatefields('{0}', '{1}', {2});", tableschema, tablename, isupdater.ToString().ToUpper());
        }

        public static string SetLogger(string tableschema, string tablename, Boolean islogger)
        {
            return String.Format("SELECT qfunc_loglogger('{0}', '{1}', {2});", tableschema, tablename, islogger.ToString().ToUpper());
        }

    }


    public class LayerRepository : ILayerRepository
    {
        private string _connectionString;

        public LayerRepository(string connstring)
        {
            _connectionString = connstring;
        }

        //layers
        public List<Layer> GetLayers()
        {
            string sql_query = " SELECT t.table_schema, t.table_name ,  " +
                                " (select obj_description((quote_ident(t.table_schema)||'.'||quote_ident(t.table_name))::regclass, 'pg_class')) descript, " +
                                " (select string_agg(gc.type, ', ') from geometry_columns gc where gc.f_table_schema = t.table_schema and gc.f_table_name = t.table_name)  as geomtype, " +
                                " case  " +
                                    " WHEN coalesce((select 1 from information_schema.triggers tr where tr.event_object_schema = t.table_schema AND tr.event_object_table = t.table_name AND tr.trigger_name = tr.event_object_table || '_log_update_trigger' limit 1),0) = 1 THEN  " +
                                        " true  " +
                                    " ELSE  " +
                                        " false " +
                                " end as isupdater, " +
                                " case  " +
                                    " WHEN coalesce((select 1 from information_schema.triggers tr where tr.event_object_schema = t.table_schema AND tr.event_object_table = t.table_name AND tr.trigger_name = tr.event_object_table || '_log_logger_trigger' limit 1),0) = 1 THEN  " +
                                        " true " +
                                    " ELSE  " +
                                        " false " +
                                " end as islogger " +
                               " FROM information_schema.tables t  " +
                               " WHERE EXISTS  (select 1 from geometry_columns gc where gc.f_table_schema = t.table_schema and gc.f_table_name = t.table_name limit 1) AND (t.table_schema not in  ('logger', 'tiger', 'schema_spr')) " +
                               " ORDER BY t.table_schema, t.table_name ; ";

            return GetListOfObjects(sql_query);
        }

        //dictionaries
        public List<Layer> GetDicts()
        {
            string sql_query = " SELECT t.table_schema, t.table_name ,  " +
                                " (select obj_description((quote_ident(t.table_schema)||'.'||quote_ident(t.table_name))::regclass, 'pg_class')) descript, " +
                                " (select string_agg(gc.type, ', ') from geometry_columns gc where gc.f_table_schema = t.table_schema and gc.f_table_name = t.table_name)  as geomtype, " +
                                " case  " +
                                    " WHEN coalesce((select 1 from information_schema.triggers tr where tr.event_object_schema = t.table_schema AND tr.event_object_table = t.table_name AND tr.trigger_name = tr.event_object_table || '_log_update_trigger' limit 1),0) = 1 THEN  " +
                                        " true  " +
                                    " ELSE  " +
                                        " false " +
                                " end as isupdater, " +
                                " case  " +
                                    " WHEN coalesce((select 1 from information_schema.triggers tr where tr.event_object_schema = t.table_schema AND tr.event_object_table = t.table_name AND tr.trigger_name = tr.event_object_table || '_log_logger_trigger' limit 1),0) = 1 THEN  " +
                                        " true " +
                                    " ELSE  " +
                                        " false " +
                                " end as islogger " +
                               " FROM information_schema.tables t  " +
                               " WHERE t.table_schema in ('schema_spr')  " +
                               " ORDER BY t.table_schema, t.table_name ; ";

            return GetListOfObjects(sql_query);
        }

        private List<Layer> GetListOfObjects(string sql_query)
        {
            var listOfObjects = new List<Layer>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sql_query, conn))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var objectpsql = new Layer();

                            objectpsql.Table_schema = dataReader["table_schema"].ToString();
                            objectpsql.Table_name = dataReader["table_name"].ToString();
                            objectpsql.Descript = dataReader["descript"].ToString();
                            objectpsql.Geomtype = dataReader["geomtype"].ToString();
                            objectpsql.Isupdater = Convert.ToBoolean(dataReader["isupdater"]);
                            objectpsql.Islogger = Convert.ToBoolean(dataReader["islogger"]);

                            listOfObjects.Add(objectpsql);
                        }
                    }
                }
            }
            return listOfObjects;
        }

        //Change layer
        public List<String> ChangeLayer(string tableschema, string tablename, string descript, bool? isupdater, bool? islogger)
        {
            List<String> sql_queries = new List<String>();

            if (descript != null) sql_queries.Add(LayerQueries.CommentOnTable(tableschema, tablename, descript));
            if (isupdater != null) sql_queries.Add(LayerQueries.SetUpdater(tableschema, tablename, (bool)isupdater));
            if (islogger != null) sql_queries.Add(LayerQueries.SetLogger(tableschema, tablename, (bool)islogger));

            try
            {
                ExecuteSqlNonQuery(sql_queries);
                return sql_queries;
            }
            catch
            {
                throw;
            }
        }

        //Execute queries
        private void ExecuteSqlNonQuery(List<string> sql_queries)
        {
            string current_query = null;
            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    foreach (string sql_query in sql_queries)
                    {
                        current_query = sql_query;
                        using (var command = new NpgsqlCommand(sql_query, conn))
                        {
                            (command.ExecuteNonQuery()).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(current_query + "\nException: " + ex.Message.ToString());
            }
        }
    }
}
