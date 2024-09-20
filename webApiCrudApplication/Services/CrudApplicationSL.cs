using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.RepositoryLayer;

namespace webApiCrudApplication.Services
{
    public class CrudApplicationSL : ICrudApplicationSL
    {
        private readonly ICrudApplicationRL _crudApplicationRL;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CrudApplicationSL(IConfiguration configuration,ICrudApplicationRL crudApplicationRL)
        {
            _crudApplicationRL = crudApplicationRL;
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:MySqlDBString"];

        }

        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse
            {
                Message = "",
                IsSuccess = true
            };

            /*// Check if the UserName in the request is null or empty
            if (string.IsNullOrEmpty(request.UserName))
            {
                response.IsSuccess = false;
                response.Message = "UserName can't be Null or Empty.";
                return response;
            }
            if (string.IsNullOrEmpty(request.EmailId))
            {
                response.IsSuccess = false;
                response.Message = "EmailId can't be Null or Empty.";
                return response;
            }
            if (string.IsNullOrEmpty(request.MobileNumber))
            {
                response.IsSuccess = false;
                response.Message = "MobileNumber can't be Null or Empty.";
                return response;
            }
            if (string.IsNullOrEmpty(request.Gender))
            {
                response.IsSuccess = false;
                response.Message = "Gender can't be Null or Empty.";
                return response;
            }
            if (request.Salary <= 0)
            {
                response.IsSuccess = false;
                response.Message = "Salry can't be less than 0.";
                return response;
            }*/

            // Continue with adding information using the repository layer
            return await _crudApplicationRL.AddInformation(request);
        }

        public async Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request)
        {
            UpdateAllInformationByIdResponse response = new UpdateAllInformationByIdResponse
            {
                Message = "", // Set a default or empty message
                IsSuccess = true
            };

            try
            {
                // Call the method on the repository layer
                response = await _crudApplicationRL.UpdateAllInformationById(request);
            }
            catch (Exception ex)
            {
                // Set failure response and capture the exception message
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request)
        {
            AddInformationResponse response = new AddInformationResponse
            {
                Message = "", // Set a default or empty message
                IsSuccess = true
            };

            return await _crudApplicationRL.DeleteInformationById(request);
        }

        public async Task<GetInformationByIdResponse> GetInformationById(int id)
        {
            // Call the repository layer to get data
            var response = await _crudApplicationRL.GetInformationById(id);

            // Return the response directly as it's already formatted in the repository layer
            return response;
        }

        public async Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            var response = new ReadAllInformationResponse
            {
                Message = "Records retrieved successfully.",
                PageIndex = pageNumber,
                PageSize = pageSize,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            try
            {
                // Fetch total record count from the repository
                response.TotalRecords = await _crudApplicationRL.GetTotalRecords(request);

                // Fetch paginated and sorted records
                var records = await _crudApplicationRL.GetPaginatedRecords(request, pageNumber, pageSize, sortBy, sortDirection);

                // Assign the records to the readAllInformation list in the response object
                response.readAllInformation = records;

                response.IsSuccess = true;
                response.Message = "Records retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }

            return response;
        }



        public LoginResponse Authenticate(LoginRequest request)
        {

            var user = _crudApplicationRL.GetUserByUsernameAndPassword(request.Username, request.Password);

            // If the user is null, authentication failed
            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password",
                    Token = null
                };
            }

            // If user is authenticated, generate the token and return it
            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login Successfully",
                Token = token
            };
        }
        public bool IsUserInRole(string username, string role)
        {
            var userRole = _crudApplicationRL.GetUserRole(username);
            return userRole != null && userRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), // Ensure this line is uncommented
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            Logger.GetInstance(_connectionString).Log("Token Generated Successfully ", string.Empty);

            return tokenHandler.WriteToken(token);

        }


    }
}


