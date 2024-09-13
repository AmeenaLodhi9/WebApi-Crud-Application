using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> ReadAllInformation()
        {
            ReadAllInformationResponse response;

            try
            {
                // Assuming Get() method in _cRudApplicationSL returns ReadAllInformationResponse
                response = await _cRudApplicationSL.ReadAllInformation(); // Removed request if not required
            }
            catch (Exception ex)
            {
                response = new ReadAllInformationResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetInformationById(int id)
        {
            var response = await _cRudApplicationSL.GetInformationById(id);

            if (response.IsSuccess)
            {
                return Ok(response);//(new { IsSuccess = response.IsSuccess, Data = response });
            }
            else
            {
                return NotFound(new { IsSuccess = response.IsSuccess, Message = response.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAllInformationById(UpdateAllInformationByIdRequest request)
        {
            UpdateAllInformationByIdResponse response = new UpdateAllInformationByIdResponse
            {
                Message = "Record Update successfully", // Set a default value to avoid the error
                IsSuccess = true

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
            return Ok(response);
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
