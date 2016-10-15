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
        /// <summary>
        /// Creates an SQL connection to the MySQL database.
        /// </summary>
        /// <param name="address">The IP/address of the MySQL database.</param>
        /// <param name="port">The port of the MySQL database.</param>
        /// <param name="username">The username to access the database.</param>
        /// <param name="password">The password to access the database.</param>
        /// <param name="database">The database name.</param>
        /// <param name="timeout">The timeout of the connection.</param>
        /// <returns>The connection instance for further use.</returns>
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

        /// <summary>
        /// Gets the SQL connections depending on the address or database provided.
        /// </summary>
        /// <param name="address">The IP of the MySQL server.</param>
        /// <param name="database">The database name.</param>
        /// <returns>Array of SQL Connections.</returns>
        public static SqlConnection[] getSQL(string address = "", string database = "")
        {
            return Array.FindAll(_connections.ToArray(), a => (!string.IsNullOrEmpty(address) ? a.DataSource == address : true) && (!string.IsNullOrEmpty(database) ? a.Database == database : true));
        }

        /// <summary>
        /// Send a custom command to the SQL database.
        /// </summary>
        /// <param name="command">The command you want to send.</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns>If the command was successfully sent.</returns>
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

        /// <summary>
        /// Reads the data specified from the MySQL database table.
        /// </summary>
        /// <param name="table">The table name to read from.</param>
        /// <param name="returnColumn">The column to get the data from.</param>
        /// <param name="checks">The array of checks to get the correct data. Example: steamId='11111111'</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns>Array of all values matching the checks. Returns null if failed.</returns>
        public static string[] sql_readCommand(string table, string returnColumn, string[] checks, SqlConnection connection)
        {
            try
            {
                string check = "";
                if (checks.Length > 0)
                {
                    check = " WHERE " + checks[0];
                    for (int i = 1; i < checks.Length; i++)
                    {
                        check += " AND " + checks[i];
                    }
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

        /// <summary>
        /// Inserts data to the table in the database.
        /// </summary>
        /// <param name="table">The table to write in.</param>
        /// <param name="columns">The columns to write in.</param>
        /// <param name="values">The values to write into the columns specified in the columns array.</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns>If the insertion was successful.</returns>
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

        /// <summary>
        /// Deletes entry from the table in the database.
        /// </summary>
        /// <param name="table">The table to edit.</param>
        /// <param name="checks">The check array to find the correct entry. Example: steamId='11111111'</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns>If the command was successul.</returns>
        public static bool sql_deleteCommand(string table, string[] checks, SqlConnection connection)
        {
            try
            {
                string check = "";
                if (checks.Length > 0)
                {
                    check = " WHERE " + checks[0];
                    for (int i = 1; i < checks.Length; i++)
                    {
                        check += " AND " + checks[i];
                    }
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

        /// <summary>
        /// Checks if a specific entry exists in the table.
        /// </summary>
        /// <param name="table">The table to check in.</param>
        /// <param name="checks">The check array to find the correct entry. Example: steamId='11111111'</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns>If it exists or not.</returns>
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

        /// <summary>
        /// Updates an entry in the table.
        /// </summary>
        /// <param name="table">The table to edit.</param>
        /// <param name="checks">The checks to find the correct entry. Example: steamId='11111111'</param>
        /// <param name="changes">The array of things to change in the entry. Example: steamId='2222222'</param>
        /// <param name="connection">The SQL Connection instance.</param>
        /// <returns></returns>
        public static bool sql_update(string table, string[] checks, string[] changes, SqlConnection connection)
        {
            try
            {
                string check = "";
                if (checks.Length > 0)
                {
                    check = " WHERE " + checks[0];
                    for (int i = 1; i < checks.Length; i++)
                    {
                        check += " AND " + checks[i];
                    }
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
        internal static bool shutdown()
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
