using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.Services;

namespace webApiCrudApplication.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CrudApplicationController : ControllerBase
    {
        public readonly ICrudApplicationSL _cRudApplicationSL;

        public CrudApplicationController(ICrudApplicationSL cRudApplicationSL)
        {
            _cRudApplicationSL = cRudApplicationSL;
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

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, Message = ex.Message });
            }

            return Ok(new { IsSuccess = response.IsSuccess, Message = response.Message });
        }


        [HttpGet]
        public async Task<IActionResult> ReadAllInformation([FromQuery] int? PageNumber, [FromQuery] int? PageSize, [FromQuery] string SortBy, [FromQuery] string SortDirection)
        {
            ReadAllInformationResponse? response = null;

            try
            {
                // Ensure PageNumber and PageSize have default values if not provided
                if (PageNumber == null || PageNumber <= 0)
                {
                    PageNumber = 1; // Default page number
                }

                if (PageSize == null || PageSize <= 0)
                {
                    PageSize = 10; // Default page size
                }

                // Create request object to pass to service layer
                var request = new GetReadAllInformationRequest
                {
                    PageNumber = PageNumber.Value,
                    PageSize = PageSize.Value,
                    SortBy = SortBy, // Pass SortBy to the request
                    SortDirection = SortDirection // Pass SortDirection to the request
                };

                // Call service layer to get the data
                response = await _cRudApplicationSL.ReadAllInformation(request);

                if (!response.IsSuccess)
                {
                    return BadRequest(new { IsSuccess = response.IsSuccess, Message = response.Message });
                }

                // Return success response with data
                return Ok(new { IsSuccess = response.IsSuccess, Data = response.readAllInformation });
            }
            catch (Exception ex)
            {
                // Handle exception and return error response
                response = new ReadAllInformationResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };

                return StatusCode(500, new { IsSuccess = response.IsSuccess, Message = response.Message });
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
            }
            catch (Exception ex)
            {
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

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(new { Data = response });
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

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response);
        }


    }

}
