using Microsoft.AspNetCore.Mvc;
using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.Services
{
    public interface ICrudApplicationSL
    {
        Task<IActionResult> Login([FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest request);

        LoginResponse Authenticate(LoginRequest request);
        //Task<AuthenticateResponse> Authenticate(string userName, string password);
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        public Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);
        public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
        public Task<GetInformationByIdResponse> GetInformationById(int id);

    }
}
