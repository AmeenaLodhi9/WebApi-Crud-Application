using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.RepositoryLayer
{
    public interface ICrudApplicationRL
    {
        Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        Task<ReadAllInformationResponse> ReadAllInformation();
        Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);

        public interface ICrudApplicationRL
        {
            public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
            public Task<ReadAllInformationResponse> ReadAllInformation();
            public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
            public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);

        }
    }
}
