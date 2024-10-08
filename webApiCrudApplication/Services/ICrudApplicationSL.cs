﻿using Microsoft.AspNetCore.Mvc;
using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.Services
{
    public interface IUserSL
    {
        LoginResponse Authenticate(LoginRequest request);
        bool IsUserInRole(string username, string role);

    }
    public interface IInformationSL
    {
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        public Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);
        public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
        public Task<GetInformationByIdResponse> GetInformationById(int id);
    }

}
