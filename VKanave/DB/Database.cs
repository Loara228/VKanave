using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKanave.DB
{
    internal class Database
    {
        public static void Initialize(string hostname, string database, string user = "root", string password = "")
        {
            sqlConnection = new MySqlConnection($"Server={hostname};Port=3306;Database={database};Uid={user};Pwd={password}");
        }

        public static bool Open(out Exception exc)
        {
            exc = null;
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch(Exception exception) { exc = exception; }
            return false;
        }

        public static Exception RunCommand(string commandStr, out SQLTable table)
        {
            Exception exc;
            table = new SQLTable();
            try
            {
                MySqlCommand commandSql = new MySqlCommand(commandStr, sqlConnection);
                if(Open(out exc))
                {
                    table.ExecuteAndRead(commandSql);
                }
                return exc;
            }
            catch(Exception exception)
            {
                return exception;
            }
        }

        public static readonly string tableSchema = "users", tableName = "userList";

        private static MySqlConnection sqlConnection;

    }
}
