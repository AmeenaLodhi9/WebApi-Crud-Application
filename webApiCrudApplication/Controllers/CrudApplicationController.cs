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
        public async Task<IActionResult>  AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse
            {
                Message = "", // Set a default value to avoid the error
                IsSuccess = true

            };

            try
            {
                response = await _cRudApplicationSL.AddInformation(request);

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAllInformationById(UpdateAllInformationByIdRequest request)
        {
            UpdateAllInformationByIdResponse response = new UpdateAllInformationByIdResponse
            {
                Message = "", // Set a default value to avoid the error
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
