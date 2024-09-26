using MySqlConnector;
using System;

namespace webApiCrudApplication.Logs
{
    public class Logger: ILogger
    {
        private readonly string _connectionString;

        public Logger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Log(string message, string stackTrace)
        {
            if (message.Length > 255)
            {
                message = message.Substring(0, 255);
            }

            Console.WriteLine($"Log: {message}, StackTrace: {stackTrace}");

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
