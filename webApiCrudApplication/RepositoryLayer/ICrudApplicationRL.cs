using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.RepositoryLayer
{
    public interface ICrudApplicationRL
    {
        Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        Task<int> GetTotalRecords(ReadAllInformationRequest request);
        Task<List<ReadAllInformationRequest>> GetPaginatedRecords(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);

        //Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);
        Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
        Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
        Task<GetInformationByIdResponse> GetInformationById(int id);

        public interface ICrudApplicationRL
        {
            public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
            public  Task<int> GetTotalRecords(ReadAllInformationRequest request);
            public Task<List<ReadAllInformationRequest>> GetPaginatedRecords(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);

           // public Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request);
            public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
            public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
            public Task<GetInformationByIdResponse> GetInformationById(int id);
        }
    }
}
