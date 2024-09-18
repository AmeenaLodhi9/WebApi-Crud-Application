using webApiCrudApplication.CommonLayer.model;
using webApiCrudApplication.RepositoryLayer;

namespace webApiCrudApplication.Services
{
    public class CrudApplicationSL : ICrudApplicationSL
    {
        private readonly ICrudApplicationRL _crudApplicationRL;
        //public readonly string EmailRegex= @"^[0-9a-zA-Z]+([._+-][0-9a-zA-Z]+)*@[0-9a-zA-Z]+.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$";
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

        /*public async Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request)
        {
            ReadAllInformationResponse response = new ReadAllInformationResponse
            {
                PageIndex = response.PageNumber,
                PageSize = response.PageSize
            };
            try
            {
                response.TotalRecords = await _crudApplicationRL.GetTotalRecords();
                // Get the paginated and sorted data from the repository
                response.ReadAllInformation = await _crudApplicationRL.GetPaginatedRecords(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection);

                response.IsSuccess = true;
                //return await _crudApplicationRL.ReadAllInformation(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
        }*/

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
    }
}


