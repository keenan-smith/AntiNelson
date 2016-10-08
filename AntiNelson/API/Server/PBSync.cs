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
        public static bool addSQL(string address, string username, string password, string database, int timeout = 30)
        {
            SqlConnection connection = new SqlConnection(
                "user id=" + username + ";" +
                "password=" + password + ";" +
                "server=" + address + ";" +
                "Trusted_Connection=yes;" +
                "database=" + database + ";" +
                "connection timeout=" + timeout + ";"
            );

            try
            {
                connection.Open();
                _connections.Add(connection);
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync!", ex, false);
                return false;
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

        public static string[] sql_readCommand(string returnColumn, string table, string checkColumn, string checkValue, SqlConnection connection)
        {
            try
            {
                List<string> rets = new List<string>();
                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand("SELECT * FROM " + table + " WHERE " + checkColumn + " = '" + checkValue + "'", connection);

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

        public static bool sql_insertCommand(string table, string columns, string values, SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO " + table + " (" + columns + ") VALUES (" + values + ");", connection);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                PBLogging.logError("ERROR in sync command!", ex, false);
                return false;
            }
        }

        public static bool sql_deleteCommand(string table, string checkColumn, string checkValue, SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM " + table + " WHERE " + checkColumn + " = '" + checkValue + "';", connection);

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
