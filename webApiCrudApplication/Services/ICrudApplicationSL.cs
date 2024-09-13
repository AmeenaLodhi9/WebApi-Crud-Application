using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.Services
{
    public interface ICrudApplicationSL
    {
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        public Task<ReadAllInformationResponse> ReadAllInformation(GetReadAllInformationRequest request);
        public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
        public Task<GetInformationByIdResponse> GetInformationById(int id);

    }
}
