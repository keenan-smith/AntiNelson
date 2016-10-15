using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API.Server
{
    public class PBSync
    {
        #region Variables
        private static List<SqlConnection> _connections = new List<SqlConnection>();
        #endregion

        #region SQL Functions
        public static SqlConnection addSQL(string address, string port, string username, string password, string database, int timeout = 30)
        {
            SqlConnection[] cons = getSQL(address, database);
            if (cons.Length > 0)
                return cons[0];

            SqlConnection connection = new SqlConnection(
                "user id=" + username + ";" +
                "password=" + password + "," + port + ";" +
                "server=" + address + ";" +
                "Trusted_Connection=yes;" +
                "database=" + database + ";" +
                "connection timeout=" + timeout + ";"
            );

            try
            {
                connection.Open();
                _connections.Add(connection);
                return connection;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync!", ex, false);
                return null;
            }
        }

        public static SqlConnection[] getSQL(string address = "", string database = "")
        {
            return Array.FindAll(_connections.ToArray(), a => (!string.IsNullOrEmpty(address) ? a.DataSource == address : true) && (!string.IsNullOrEmpty(database) ? a.Database == database : true));
        }

        public static bool sql_sendCommand(string command, SqlConnection connection)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(command, connection);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }

        public static string[] sql_readCommand(string table, string returnColumn, string[] checks, SqlConnection connection)
        {
            try
            {
                string check = " WHERE " + checks[0];
                for (int i = 1; i < checks.Length; i++)
                {
                    check += " AND " + checks[i];
                }

                List<string> rets = new List<string>();
                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand("SELECT * FROM " + table + check + ";", connection);

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rets.Add(reader[returnColumn].ToString());
                }

                return rets.ToArray();
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return null;
            }
        }

        public static bool sql_insertCommand(string table, string[] columns, string[] values, SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO " + table + " (" + string.Join(",", columns) + ") VALUES (" + string.Join(",", columns) + ");", connection);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }

        public static bool sql_deleteCommand(string table, string[] checks, SqlConnection connection)
        {
            try
            {
                string check = " WHERE " + checks[0];
                for (int i = 1; i < checks.Length; i++)
                {
                    check += " AND " + checks[i];
                }

                SqlCommand command = new SqlCommand("DELETE FROM " + table + check + ";", connection);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }

        public static bool sql_exists(string table, string[] checks, SqlConnection connection)
        {
            try
            {
                return sql_readCommand(table, "steamId", checks, connection).Length > 0;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }

        public static bool sql_update(string table, string[] checks, string[] changes, SqlConnection connection)
        {
            try
            {
                string check = " WHERE " + checks[0];
                for (int i = 1; i < checks.Length; i++)
                {
                    check += " AND " + checks[i];
                }

                SqlCommand command = new SqlCommand("UPDATE " + table + " SET " + string.Join(",", changes) + check + ";", connection);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }
        #endregion

        #region Functions
        public static bool shutdown()
        {
            bool success = true;

            if (_connections.Count > 0)
            {
                foreach (SqlConnection connection in _connections)
                {
                    try
                    {
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        PBLogging.logError("ERROR in sync shutdown!", ex, false);
                        success = false;
                    }
                }
            }

            return success;
        }
        #endregion
    }
}
