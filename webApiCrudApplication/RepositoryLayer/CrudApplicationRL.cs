using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using webApiCrudApplication.Common_Util;
using webApiCrudApplication.CommonLayer.model;



namespace webApiCrudApplication.RepositoryLayer
{
    public class CrudApplicationRL : ICrudApplicationRL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        private readonly Logger _logger;

        public CrudApplicationRL(IConfiguration configuration, Logger logger)
        {
            _configuration = configuration;

            _logger = logger;

            // Retrieve the connection string from the configuration
            string myConnection = _configuration.GetConnectionString("MySqlDBString");

            // Null-check the connection string
            if (string.IsNullOrEmpty(myConnection))
            {
                throw new InvalidOperationException("MySQL connection string is missing or empty.");
            }

            // Initialize MySqlConnection with the connection string
            _mySqlConnection = new MySqlConnection(myConnection);
        }



        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse
            {
                /*                response.IsSuccess = true,
                                response.Message = "Success"*/
                Message = "Success", // Set a default or empty message
                IsSuccess = true
            };

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQueries.AddInformation, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", request.EmailId);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    int status = await sqlCommand.ExecuteNonQueryAsync();
                    if (status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Query Not Executed";
                    }
                   // _logger.Log("Add Information Successfully", null);

                }
               

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
               
                //_logger.Log(ex.Message,ex.StackTrace);

            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<int> GetTotalRecords(ReadAllInformationRequest request)
        {

            if (_mySqlConnection.State != System.Data.ConnectionState.Open)
            {
                await _mySqlConnection.OpenAsync();
            }

            string query = "SELECT COUNT(*) FROM crudoperation.crudapplication WHERE IsActive = 1";

            if (!string.IsNullOrEmpty(request.UserName))
            {
                query += " AND UserName LIKE @UserName";
            }

            if (!string.IsNullOrEmpty(request.EmailId))
            {
                query += " AND EmailId LIKE @EmailId";
            }

            if (request.Salary.HasValue)
            {
                query += " AND Salary = @Salary";
            }
            if (!string.IsNullOrEmpty(request.Gender))
            {
                query += " AND Gender LIKE @Gender ";
            }

            using (var command = new MySqlCommand(query, _mySqlConnection))
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    command.Parameters.AddWithValue("@UserName", $"%{request.UserName}%");
                }

                if (!string.IsNullOrEmpty(request.EmailId))
                {
                    command.Parameters.AddWithValue("@EmailId", $"%{request.EmailId}%");
                }

                if (request.Salary.HasValue)
                {
                    command.Parameters.AddWithValue("@Salary", request.Salary.Value);
                }
                if (!string.IsNullOrEmpty(request.Gender))
                {
                    command.Parameters.AddWithValue("@Gender", $"%{request.Gender}%");
                }

                return Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        public async Task<List<ReadAllInformationRequest>> GetPaginatedRecords(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            if (_mySqlConnection.State != System.Data.ConnectionState.Open)
            {
                await _mySqlConnection.OpenAsync();
            }

            // Validate and sanitize sorting inputs
            var allowedSortBy = new List<string> { "UserId", "UserName", "EmailId", "Salary" }; // Add more allowed columns
            var allowedSortDirection = new List<string> { "ASC", "DESC" };

            sortBy = allowedSortBy.Contains(sortBy) ? sortBy : "UserId"; // Default to UserId
            sortDirection = allowedSortDirection.Contains(sortDirection?.ToUpper()) ? sortDirection.ToUpper() : "ASC"; // Default to ASC

            string query = "SELECT * FROM crudoperation.crudapplication WHERE IsActive = 1";

            if (!string.IsNullOrEmpty(request.UserName))
            {
                query += " AND UserName LIKE @UserName";
            }

            if (!string.IsNullOrEmpty(request.EmailId))
            {
                query += " AND EmailId LIKE @EmailId";
            }

            if (request.Salary.HasValue)
            {
                query += " AND Salary = @Salary";
            }
            if (!string.IsNullOrEmpty(request.Gender))
            {
                query += " AND Gender LIKE @Gender";
            }


            query += $" ORDER BY {sortBy} {sortDirection}";

            int offset = (pageNumber - 1) * pageSize;
            query += " LIMIT @PageSize OFFSET @Offset";

            using (var command = new MySqlCommand(query, _mySqlConnection))
            {
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    command.Parameters.AddWithValue("@UserName", $"%{request.UserName}%");
                }

                if (!string.IsNullOrEmpty(request.EmailId))
                {
                    command.Parameters.AddWithValue("@EmailId", $"%{request.EmailId}%");
                }

                if (request.Salary.HasValue)
                {
                    command.Parameters.AddWithValue("@Salary", request.Salary.Value);
                }
                if (!string.IsNullOrEmpty(request.Gender))
                {
                    command.Parameters.AddWithValue("@Gender", $"%{request.Gender}%");
                }

                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", offset);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var result = new List<ReadAllInformationRequest>();
                    while (await reader.ReadAsync())
                    {
                        result.Add(new ReadAllInformationRequest
                        {
                            UserID = reader.GetInt32("UserId"),
                            UserName = reader.GetString("UserName"),
                            EmailId = reader.GetString("EmailId"),
                            MobileNumber = reader.GetString("MobileNumber"),
                            Salary = reader.GetInt32("Salary"),
                            Gender = reader.GetString("Gender"),
                            IsActive = reader.GetBoolean("IsActive")
                        });
                    }
                    return result;
                }
            }
        }

        public async Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request)

        {
            UpdateAllInformationByIdResponse response = new UpdateAllInformationByIdResponse
            {
                /*                response.IsSuccess = true,
                                response.Message = "Success"*/
                Message = "Success", // Set a default or empty message
                IsSuccess = true
            };
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQueries.UpdateAllInformationById, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserID", request.UserID);
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailId", request.EmailId);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    int status = await sqlCommand.ExecuteNonQueryAsync();
                    if (status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Query Not Executed";
                    }
                    



                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                


            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;


        }

        public async Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request)
        {
            DeleteInformationByIdResponse response = new DeleteInformationByIdResponse
            {
                /*                response.IsSuccess = true,
                                response.Message = "Success"*/
                Message = "Success", // Set a default or empty message
                IsSuccess = true
            };
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQueries.DeleteInformationById, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
   
                    int status = await sqlCommand.ExecuteNonQueryAsync();
                    if (status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Query Not Executed";
                    }


                }
               

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
              


            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        public async Task<GetInformationByIdResponse> GetInformationById(int id)
        {
            // Initialize the response object with default values
            GetInformationByIdResponse response = new GetInformationByIdResponse
            {
                Message = "Data Retrieved Succesfully ", // Set a default or empty message
                IsSuccess = true,
            };

            try
            {
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQueries.GetInformationById, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@UserID", id);

                    if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                    {
                        await _mySqlConnection.OpenAsync();
                    }

                    using (MySqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                GetInformationByIdRequest getdata = new GetInformationByIdRequest
                                {
                                    UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? default : reader.GetInt32(reader.GetOrdinal("UserID")),
                                    UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? "DefaultUserName" : reader.GetString(reader.GetOrdinal("UserName")),
                                    EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? "DefaultEmailId" : reader.GetString(reader.GetOrdinal("EmailId")),
                                    MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? "DefaultMobileNumber" : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetInt32(reader.GetOrdinal("Salary")),
                                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? "DefaultGender" : reader.GetString(reader.GetOrdinal("Gender")),
                                    IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? default : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };

                                response.Data = getdata; // Ensure mapping here
                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Record Not Found";
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                

            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }



        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User user = null;

            using (var command = new MySqlCommand("SELECT * FROM Users WHERE Username = @Username AND Password = @Password",_mySqlConnection))
            {
                //string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

                
                    _mySqlConnection.Open();

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                
            }

            return user;
        }

        public string GetUserRole(string username)
        {
            string role = null;
            string query = "SELECT Role FROM Users WHERE Username = @Username";

            using (MySqlCommand cmd = new MySqlCommand(query, _mySqlConnection))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                _mySqlConnection.Open();

                role = cmd.ExecuteScalar()?.ToString();

                _mySqlConnection.Close();
            }
            return role;
        }

        
    }
}
