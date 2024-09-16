using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using webApiCrudApplication.Common_Util;
using webApiCrudApplication.CommonLayer.model;


namespace webApiCrudApplication.RepositoryLayer
{
    public class CrudApplicationRL : ICrudApplicationRL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        public CrudApplicationRL(IConfiguration configuration)
        {
            _configuration = configuration;

            // Retrieve the connection string
            string myConnection = _configuration["ConnectionStrings:MySqlDBString"];

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



        public async Task<ReadAllInformationResponse> ReadAllInformation(GetReadAllInformationRequest request)
        {
            ReadAllInformationResponse response = new ReadAllInformationResponse
            {
                Message = "", // Set a default value to avoid the error
                IsSuccess = true
            };

            try
            {
                int pageSize = request.PageSize ?? 10;
                int pageNumber = request.PageNumber ?? 1;
                int offset = (pageNumber - 1) * pageSize;

                string sortBy = string.IsNullOrEmpty(request.SortBy) ? "UserID" : request.SortBy;
                string sortDirection = request.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";

                // Validate sort column to prevent SQL injection
                var validSortColumns = new List<string> { "UserID", "UserName", "EmailId", "Salary", "Gender" };
                if (!validSortColumns.Contains(sortBy))
                {
                    sortBy = "UserID";
                }

                // Build the base SQL query
                string query = @"
                    SELECT * 
                    FROM crudoperation.crudapplication
                    WHERE IsActive = 1
                ";

                // Dynamically add filters
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    query += " AND UserName LIKE @UserName ";
                }

                if (!string.IsNullOrEmpty(request.EmailId))
                {
                    query += " AND EmailId LIKE @EmailId ";
                }

                if (request.Salary > 0)
                {
                    query += " AND Salary = @Salary ";
                }

                if (!string.IsNullOrEmpty(request.Gender))
                {
                    query += " AND Gender = @Gender ";
                }

                // Apply sorting and paging
                query += $" ORDER BY {sortBy} {sortDirection} LIMIT @PageSize OFFSET @Offset;";

                using (MySqlCommand sqlCommand = new MySqlCommand(query, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    // Add paging parameters
                    sqlCommand.Parameters.AddWithValue("@PageSize", pageSize);
                    sqlCommand.Parameters.AddWithValue("@Offset", offset);

                    // Add filtering parameters
                    if (!string.IsNullOrEmpty(request.UserName))
                    {
                        sqlCommand.Parameters.AddWithValue("@UserName", $"%{request.UserName}%");
                    }

                    if (!string.IsNullOrEmpty(request.EmailId))
                    {
                        sqlCommand.Parameters.AddWithValue("@EmailId", $"%{request.EmailId}%");
                    }

                    if (request.Salary > 0)
                    {
                        sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    }

                    if (!string.IsNullOrEmpty(request.Gender))
                    {
                        sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    }

                    if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                    {
                        await _mySqlConnection.OpenAsync();
                    }

                    using (MySqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            response.readAllInformation = new List<GetReadAllInformationRequest>();
                            while (await reader.ReadAsync())
                            {
                                GetReadAllInformationRequest getdata = new GetReadAllInformationRequest
                                {
                                    UserID = reader.IsDBNull(reader.GetOrdinal("UserID")) ? default : reader.GetInt32(reader.GetOrdinal("UserID")),
                                    UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? "DefaultUserName" : reader.GetString(reader.GetOrdinal("UserName")),
                                    EmailId = reader.IsDBNull(reader.GetOrdinal("EmailId")) ? "DefaultEmailId" : reader.GetString(reader.GetOrdinal("EmailId")),
                                    MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? "DefaultMobileNumber" : reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? "DefaultGender" : reader.GetString(reader.GetOrdinal("Gender")),
                                    Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetInt32(reader.GetOrdinal("Salary")),
                                    IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? default : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };

                                response.readAllInformation.Add(getdata);
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

            return response; // Ensure that a value is returned from all paths
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
    }
}
