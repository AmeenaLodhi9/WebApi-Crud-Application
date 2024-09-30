using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                // Open a new MySQL connection
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL query to insert the log into the Logs table
                    var query = "INSERT INTO crudoperation.Logs (Message, StackTrace, CurrentTime) VALUES (@Message, @StackTrace, @CurrentTime)";

                    // Execute the query
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Message", message);
                        command.Parameters.AddWithValue("@StackTrace", stackTrace);
                        command.Parameters.AddWithValue("@CurrentTime", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, maybe log it to a file or display it
                Console.WriteLine($"Error while logging: {ex.Message}");
            }

        }
    }
}
