﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.Services;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace webApiCrudApplication.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CrudApplicationController : ControllerBase
    {
        public readonly ICrudApplicationSL _cRudApplicationSL;
        private readonly Logger _logger;
         
        public CrudApplicationController(ICrudApplicationSL cRudApplicationSL, Logger logger)
        {
            _cRudApplicationSL = cRudApplicationSL;
            _logger = logger;
        }
        [HttpPost]
        public IActionResult Login([FromBody] CommonLayer.model.LoginRequest request)
        {
            var response = _cRudApplicationSL.Authenticate(request);
            _logger.Log("Login Successfully", null);


            if (response == null)
            {
                _logger.Log("Invalid username or password", null);

                return Unauthorized(new
                {
                    IsSuccess = false,
                    Message = "Invalid username or password",
                    Token = (string)null

                });
            }

            return Ok(new
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                Token = response.Token
            });
        }

        // Example of a protected route
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only Admin role can access this
        public IActionResult GetAdminData()
        {
            var Data = new[]
            {
                new { Id = 1, Name = "Admin User 1" },
                new { Id = 2, Name = "Admin User 2" }
            };

            _logger.Log("You are authorized as Admin!", null);

            return Ok(new { IsSuccess = true, Message= "You are authorized as Admin!",Data });
        }

        [HttpGet]
        [Authorize(Roles = "User")] // Only Admin role can access this
        public IActionResult GetUserData()
        {
            var Data = new[]
            {
                new { Id = 1, Name = "User User 1" },
                new { Id = 2, Name = "User User 2" }
            };

            _logger.Log("You are authorized as User!", null);

            return Ok(new { IsSuccess = true, Message = "You are authorized as User!", Data });
        }

        [HttpPost]
        public async Task<IActionResult> AddInformation([FromBody] AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse
            {
                IsSuccess = true,
                Message = "" // Default empty message
            };

            // Manually check for null or invalid values for each field

            // Check for UserName
            if (string.IsNullOrEmpty(request.UserName) || request.UserName == "string")
            {
                response.IsSuccess = false;
                response.Message = "UserName is required.";
                return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }

            // Check for EmailId
            if (string.IsNullOrEmpty(request.EmailId) || request.EmailId == "string")
            {
                response.IsSuccess = false;
                response.Message = "EmailId is required or should not be 'string'.";
                return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }

            // Check for MobileNumber
            if (string.IsNullOrEmpty(request.MobileNumber) || request.MobileNumber == "string")
            {
                response.IsSuccess = false;
                response.Message = "MobileNumber is required or should not be 'string'.";
                return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }

            // Check for Salary
            if (request.Salary <= 0)
            {
                response.IsSuccess = false;
                response.Message = "Salary must be greater than 0.";
                return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }

            // Check for Gender
            if (string.IsNullOrEmpty(request.Gender) || request.Gender == "string")
            {
                response.IsSuccess = false;
                response.Message = "Gender is required or should not be 'string'.";
                return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }

            try
            {
                // Call the service layer to add the information
                response = await _cRudApplicationSL.AddInformation(request);
                _logger.Log("Add Information Successfully", null);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
                _logger.Log(ex.Message, ex.StackTrace);
            }

            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message });
        }

        [HttpGet]
        public async Task<IActionResult> ReadAllInformation(
        [FromQuery] int? PageNumber,
        [FromQuery] int? PageSize,
        [FromQuery] string SortBy = null,
        [FromQuery] string SortDirection = null,
        [FromQuery] string UserName = null,  // Optional
        [FromQuery] string EmailId = null,   // Optional
        [FromQuery] int? Salary = null,      // Optional
        [FromQuery] string Gender = null)   // Optional)   // New filter parameter
        {
            try
            {
                // Ensure PageNumber and PageSize have default values if not provided
                PageNumber ??= 1; // Default page number
                PageSize ??= 10;  // Default page size

                // Create the request object based on input
                var request = new ReadAllInformationRequest
                {
                    UserName = UserName,
                    EmailId = EmailId,
                    Salary = Salary,
                    Gender = Gender,
                    IsActive = true // Assuming only active users are retrieved
                };

                // Call service layer to get the data
                ReadAllInformationResponse response = await _cRudApplicationSL.ReadAllInformation(request, PageNumber.Value, PageSize.Value, SortBy, SortDirection);

                // Check if no records are found
                if (response.readAllInformation == null || !response.readAllInformation.Any())
                {
                    // Return 404 if no records found
                    return NotFound(new { IsSuccess = false, Message = "No records found." });
                }

                // Check for unsuccessful response
                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
                }
                _logger.Log("Read All Information Successfully", null);


                // Return success response with data, including pagination info
                return Ok(new
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Data = response.readAllInformation,
                    
                    TotalRecords = response.TotalRecords,
                    PageIndex = response.PageIndex,
                    PageSize = response.PageSize
                });

            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, null);

                // Handle exception and return error response
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetInformationById(int id)
        {
            GetInformationByIdResponse response = new GetInformationByIdResponse
            {
                Message = ""
            };

            try
            {
                // Call service layer to get the information
                response = await _cRudApplicationSL.GetInformationById(id);

                if (response.IsSuccess)
                {
                    return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message, Data = response.Data });
                }
                else
                {
                    return NotFound(new { IsSuccess = response.IsSuccess, Message = response.Message });
                }
                _logger.Log("GetInformationById Successfully", null);

            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, null);

                // Handle exceptions and return internal server error
                return StatusCode(500, new { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAllInformationById(UpdateAllInformationByIdRequest request)
        {
            UpdateAllInformationByIdResponse response = new UpdateAllInformationByIdResponse
            {
                Message = "Record Update successfully", // Set a default value to avoid the error
                //IsSuccess = true

            };

            try
            {
                response = await _cRudApplicationSL.UpdateAllInformationById(request);
                _logger.Log("UpdateAllInformationById Successfully!", null);


            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, null);

                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response );
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInfomationById(DeleteInformationByIdRequest request)
        {
            DeleteInformationByIdResponse response = new DeleteInformationByIdResponse
            {
                Message = "", // Set a default value to avoid the error
                IsSuccess = true

            };

            try
            {
                response = await _cRudApplicationSL.DeleteInformationById(request); 
                _logger.Log("DeleteInformationById Successfully", null);



            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, null);

                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response);
        }


    }

}
