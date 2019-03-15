﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using QConsole.DAL.AccessLayer.Entities;
using QConsole.DAL.AccessLayer.Interfaces;

namespace QConsole.DAL.AccessLayer.Repositories
{

    // queries for Grants
    public static class GrantsQueries
    {
        public static string RevokeAllOnTable(string tableschema, string tablename, string role)
        {
            return String.Format("REVOKE ALL ON TABLE \"{0}\".\"{1}\" FROM GROUP {2};", tableschema, tablename, role);
        }

        public static string GrantOnTable(string tableschema, string tablename, string role, string grants)
        {
            return String.Format("GRANT {0} ON TABLE \"{1}\".\"{2}\" TO {3};", grants, tableschema, tablename, role);
        }

        public static string RevokeAllOnSeq(string tableschema, string seq, string role)
        {
            return String.Format("REVOKE ALL ON SEQUENCE \"{0}\".\"{1}\" FROM GROUP {2};", tableschema, seq, role);
        }

        public static string GrantOnSeq(string tableschema, string seq, string role)
        {
            return String.Format("GRANT ALL ON SEQUENCE \"{0}\".\"{1}\" TO GROUP {2};", tableschema, seq, role);
        }
    }

    public class GrantRepository : IGrantRepository
    {

        private string _connectionString;

        public GrantRepository(string connstring)
        {
            _connectionString = connstring;
        }

        //groups
        public List<User> GetGroups()
        {
            string sql_query = " select pg.groname as rolname, (select shobj_description(pg.grosysid, 'pg_authid')) as descript, pg.grosysid as sysid, " +
                            " 1 as isrole, 0 as usesuper " +
                            " FROM pg_group pg WHERE pg.groname not in ('pg_signal_backend') " +
                            " ORDER BY pg.groname;";
            return GetListOfRoles(sql_query);
        }

        //users in selected group
        public List<User> GetUsers(string grosysid)
        {
            string sql_query = String.Format(" select pr.rolname, (select shobj_description(pr.oid, 'pg_authid')) as descript,  pr.rolsuper as usesuper, " +
                                        " CASE WHEN EXISTS(SELECT 1 from pg_user p where p.usesysid = pr.oid) THEN 0 else 1 END as isrole, " +
                                        " pr.oid as sysid " +
                                        " from pg_roles pr" +
                                        " left join pg_auth_members am on pr.oid = am.member " +
                                        " left join pg_group pg on am.roleid = pg.grosysid " +
                                        " where pg.grosysid = {0} " +
                                        " ORDER BY pr.rolname", grosysid);
            return GetListOfRoles(sql_query);
        }

        //get list of Groups/Users
        private List<User> GetListOfRoles(string sql_query)
        {
            var listOfObjects = new List<User>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sql_query, conn))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var objectpsql = new User();

                            objectpsql.Usename = dataReader["rolname"].ToString();
                            objectpsql.Descript = dataReader["descript"].ToString();
                            objectpsql.Usesuper = Convert.ToBoolean(dataReader["usesuper"]);
                            objectpsql.Isrole = Convert.ToBoolean(dataReader["isrole"]);
                            objectpsql.Usesysid = dataReader["sysid"].ToString();

                            listOfObjects.Add(objectpsql);
                        }
                    }
                }
            }
            return listOfObjects;
        }

        //layers list for selected grantee (role)
        public List<Grant> GetLayers(string grantee)
        {
            string sql_query = String.Format(" WITH grants AS(SELECT  " +
                        " rtg.grantee, rtg.table_schema, rtg.table_name, (select obj_description((quote_ident(rtg.table_schema) || '.' || quote_ident(rtg.table_name))::regclass, 'pg_class')) descript, " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'INSERT') " +
                                " THEN 1 ELSE 0 END AS isinsert," +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'SELECT') " +
                                " THEN 1 ELSE 0 END AS isselect, " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'UPDATE') " +
                                " THEN 1 ELSE 0 END AS isupdate, " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'DELETE') " +
                                " THEN 1 ELSE 0 END AS isdelete " +
                    " FROM    information_schema.role_table_grants rtg " +
                    " WHERE EXISTS  (select 1 from geometry_columns gc where gc.f_table_schema = rtg.table_schema and gc.f_table_name = rtg.table_name limit 1) AND(rtg.table_name <> 'logtable') AND rtg.grantee = '{0}' " +
                    " GROUP BY rtg.grantee, rtg.table_schema, rtg.table_name) " +
                      " (SELECT " +
                      " ps.table_schema, ps.table_name, ps.descript, " +
                      " case when ps.isselect = 1 then true else false end as isselect, " +
                      " case when ps.isupdate = 1 then true else false end as isupdate, " +
                      " case when ps.isinsert = 1 then true else false end as isinsert, " +
                      " case when ps.isdelete = 1 then true else false end as isdelete, " +
                      " ps.grantee " +
                      " FROM(  " +
                    " SELECT t.table_schema, t.table_name , (select obj_description((quote_ident(t.table_schema) || '.' || quote_ident(t.table_name))::regclass, 'pg_class')) as descript,  " +
                        " gr.isselect, gr.isupdate, gr.isinsert, gr.isdelete, gr.grantee " +
                        " FROM information_schema.tables t LEFT JOIN  grants gr ON gr.table_schema||gr.table_name = t.table_schema||t.table_name " +
                        " WHERE EXISTS  (select 1 from geometry_columns gc where gc.f_table_schema = t.table_schema and gc.f_table_name = t.table_name limit 1) AND (t.table_schema not in  ('logger', 'tiger', 'schema_spr'))" +
                        " ORDER BY t.table_schema, t.table_name) ps);", grantee);
            return GetListOfTables(sql_query);
        }

        //dicts
        public List<Grant> GetDicts(string grantee)
        {
            string sql_query = String.Format(" WITH grants AS(SELECT   " +
                        " rtg.grantee, rtg.table_schema, rtg.table_name, (select obj_description((quote_ident(rtg.table_schema) || '.' || quote_ident(rtg.table_name))::regclass, 'pg_class')) descript,  " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'INSERT')  " +
                                " THEN 1 ELSE 0 END AS isinsert, " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'SELECT')  " +
                                " THEN 1 ELSE 0 END AS isselect,  " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'UPDATE')  " +
                                " THEN 1 ELSE 0 END AS isupdate,  " +
                            " CASE WHEN exists(select 1 from information_schema.role_table_grants where grantee = rtg.grantee AND rtg.table_schema=table_schema AND rtg.table_name=table_name and privilege_type = 'DELETE')  " +
                                " THEN 1 ELSE 0 END AS isdelete  " +
                    " FROM    information_schema.role_table_grants rtg  " +
                    " WHERE (rtg.table_schema = 'schema_spr') AND rtg.grantee = '{0}'  " +
                    " GROUP BY rtg.grantee, rtg.table_schema, rtg.table_name)  " +
                      " (SELECT  " +
                      " ps.table_schema, ps.table_name, ps.descript,  " +
                      " case when ps.isselect = 1 then true else false end as isselect,  " +
                      " case when ps.isupdate = 1 then true else false end as isupdate,  " +
                      " case when ps.isinsert = 1 then true else false end as isinsert,  " +
                      " case when ps.isdelete = 1 then true else false end as isdelete,  " +
                      " ps.grantee  " +
                      " FROM(   " +
                    " SELECT t.table_schema, t.table_name , (select obj_description((quote_ident(t.table_schema) || '.' || quote_ident(t.table_name))::regclass, 'pg_class')) as descript,   " +
                        " gr.isselect, gr.isupdate, gr.isinsert, gr.isdelete, gr.grantee  " +
                        " FROM information_schema.tables t LEFT JOIN  grants gr ON gr.table_schema||gr.table_name = t.table_schema||t.table_name  " +
                        " WHERE (t.table_schema = 'schema_spr') " +
                        " ORDER BY t.table_schema, t.table_name) ps); ", grantee);
            return GetListOfTables(sql_query);
        }

        //get list of Groups/Users
        private List<Grant> GetListOfTables(string sql_query)
        {
            var listOfObjects = new List<Grant>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sql_query, conn))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var objectpsql = new Grant();

                            objectpsql.Table_schema = dataReader["table_schema"].ToString();
                            objectpsql.Table_name = dataReader["table_name"].ToString();
                            objectpsql.Descript = dataReader["descript"].ToString();
                            objectpsql.IsSelect = Convert.ToBoolean(dataReader["isselect"]);
                            objectpsql.IsUpdate = Convert.ToBoolean(dataReader["isupdate"]);
                            objectpsql.IsInsert = Convert.ToBoolean(dataReader["isinsert"]);
                            objectpsql.IsDelete = Convert.ToBoolean(dataReader["isdelete"]);

                            listOfObjects.Add(objectpsql);
                        }
                    }
                }
            }
            return listOfObjects;
        }

        private List<List<string>> GetListOfSeq(string table_schema, string table_name)
        {
            string sql_query = String.Format(" WITH fq_objects AS (SELECT c.oid, c.relnamespace, n.nspname as nspace,c.relname AS fqname, c.relkind, c.relname AS relation   " +
                            " FROM pg_class c JOIN pg_namespace n ON n.oid = c.relnamespace ),  " +
                            " sequences AS(SELECT oid, relnamespace, nspace, fqname FROM fq_objects WHERE relkind = 'S'),   " +
                            " tables AS(SELECT oid, relnamespace, nspace, fqname FROM fq_objects WHERE relkind = 'r')  " +
                            " SELECT  " +
                                " s.nspace as seqspace, s.relnamespace as seqspaceid, s.fqname AS sequence, t.nspace as tabspace, t.relnamespace as tabspaceid, t.fqname AS tablename  " +
                            " FROM  pg_depend d JOIN sequences s ON s.oid = d.objid JOIN tables t ON t.oid = d.refobjid  " +
                            " WHERE d.deptype = 'a' and t.nspace = '{0}' and t.fqname = '{1}'; ", table_schema, table_name);


            var listOfObjects = new List<List<string>>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sql_query, conn))
                {
                    using (var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            List<string> seq = new List<string>();
                            string seqspace = dataReader["seqspace"].ToString();
                            string seqname = dataReader["sequence"].ToString();
                            seq.Add(seqspace);
                            seq.Add(seqname);
                            listOfObjects.Add(seq);
                        }
                    }
                }
            }
            return listOfObjects;
        }

        //Grant privileges to selected role
        public List<string> GrantTableToRole(string table_schema, string table_name, string role, List<string> grants_list)
        {
            List<string> sql_queries = new List<String>();
            sql_queries.Add(GrantsQueries.RevokeAllOnTable(table_schema, table_name, role));
            //sequences list
            List<List<string>> seq_list = GetListOfSeq(table_schema, table_name);
            foreach (List<string> seq in seq_list)
            {
                string seqspace = seq[0];
                string seqname = seq[1];
                sql_queries.Add(GrantsQueries.RevokeAllOnSeq(seqspace, seqname, role)); //Revoke sequences
            }

            if (grants_list.Count > 0)
            {
                string grants = string.Join(", ", grants_list);

                sql_queries.Add(GrantsQueries.GrantOnTable(table_schema, table_name, role, grants));

                foreach (List<string> seq in seq_list)
                {
                    string seqspace = seq[0];
                    string seqname = seq[1];
                    if (grants_list.Count > 0) sql_queries.Add(GrantsQueries.GrantOnSeq(seqspace, seqname, role)); //Grant sequences
                }
            }
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