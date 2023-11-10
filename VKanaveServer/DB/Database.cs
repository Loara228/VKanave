using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKanaveServer;

namespace VKanave.DB
{
    internal static class Database
    {
        internal static bool Initialize(string hostname, out Exception exception, string user = "root", string password = "")
        {
            _sqlConnection = new MySqlConnection($"Server={hostname};Port=3306;Database=vk;Uid={user};Pwd={password}");
            bool result = Open(out exception);
            if (result)
                Close();
            Program.Log(LogType.SQL, "Initialized");
            return result;
        }

        private static bool Open(out Exception exc)
        {
            exc = null;
            try
            {
                _sqlConnection.Open();
                return true;
            }
            catch (Exception exception) { exc = exception; }
            return false;
        }

        private static void Close() => _sqlConnection.Close();

        internal static long GetUnixTime()
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow);
            return dto.ToUnixTimeSeconds();
        }
        internal static DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixtime);
            return dtDateTime;
        }

        internal static Exception RunCommand(string commandStr, out SQLTable table)
        {
            Program.Log(LogType.SQL, commandStr);
            Exception exc;
            table = new SQLTable();
            try
            {
                MySqlCommand commandSql = new MySqlCommand(commandStr, _sqlConnection);
                if (Open(out exc))
                {
                    table.ExecuteAndRead(commandSql);
                    Close();
                }
                else
                {
                    if (exc != null)
                    {
                        Close();
                        Program.Log(LogType.SQL, exc.Message, true);
                    }
                }
                return exc;
            }
            catch (Exception exception)
            {
                if (exception != null)
                    Program.Log(LogType.SQL, exception.Message, true);
                Close();
                return exception;
            }
        }

        internal static string SQLHostname
        {
            get => Program.LOCAL ? "localhost" : "?";
        }

        internal static string SQLUsername
        {
            get => Program.LOCAL ? "root" : "?";
        }

        internal static string SQLPassword
        {
            get => Program.LOCAL ? "" : "?";
        }

        private static MySqlConnection _sqlConnection;

    }
}
