using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.RepositoryLayer
{
    public interface ICrudApplicationRL
    {
        Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        Task<ReadAllInformationResponse> ReadAllInformation(GetReadAllInformationRequest request);
        Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
        Task<GetInformationByIdResponse> GetInformationById(int id);

        public interface ICrudApplicationRL
        {
            public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
            public Task<ReadAllInformationResponse> ReadAllInformation(GetReadAllInformationRequest request);
            public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
            public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
            public Task<GetInformationByIdResponse> GetInformationById(int id);
        }
    }
}
