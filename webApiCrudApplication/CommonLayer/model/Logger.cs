using MySqlConnector;
using System;

namespace webApiCrudApplication.CommonLayer.model
{
    public class Logger
    {
        private readonly string _connectionString;

        // Constructor accepts the connection string, injected by the DI container
        public Logger(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Log method that inserts logs into the database
        public void Log(string message, string stackTrace)
        {
            Console.WriteLine($"Log: {message}, StackTrace: {stackTrace}");
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
