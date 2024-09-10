using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.RepositoryLayer;

namespace webApiCrudApplication.Services
{
    public class CrudApplicationSL : ICrudApplicationSL
    {
        private readonly ICrudApplicationRL _crudApplicationRL;

        public CrudApplicationSL(ICrudApplicationRL crudApplicationRL)
        {
            _crudApplicationRL = crudApplicationRL;
        }

        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            // Initialize the Message property in the object initializer
            AddInformationResponse response = new AddInformationResponse
            {
                Message = "", // Set a default or empty message
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



        public async Task<ReadAllInformationResponse> ReadAllInformation()
        {
            try
            {
                return await _crudApplicationRL.ReadAllInformation();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                var response = new ReadAllInformationResponse
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
                return response;
            }
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


        // Implement the methods from ICrudApplicationSL interface here
        public async Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request)
        {
            // Initialize the Message property in the object initializer
            AddInformationResponse response = new AddInformationResponse
            {
                Message = "", // Set a default or empty message
                IsSuccess = true
            };


            // Continue with adding information using the repository layer
            return await _crudApplicationRL.DeleteInformationById(request);
        }
    }
}
