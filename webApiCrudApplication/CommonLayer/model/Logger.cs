using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace webApiCrudApplication.CommonLayer.model
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private readonly string _connectionString;

        private Logger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static Logger GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger(connectionString);
                    }
                }
            }
            return _instance;
        }

        public void Log(string message, string stackTrace)
        {
            if (message.Length > 255)
            {
                message = message.Substring(0, 255);
            }
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO crudoperation.Logs (Message, StackTrace, CurrentTime) VALUES (@Message, @StackTrace, @CurrentTime)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Message", message);
                    command.Parameters.AddWithValue("@StackTrace", stackTrace);
                    command.Parameters.AddWithValue("@CurrentTime", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
