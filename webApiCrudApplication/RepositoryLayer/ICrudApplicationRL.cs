using webApiCrudApplication.CommonLayer.model;

namespace webApiCrudApplication.RepositoryLayer
{

        public interface IUserRL
        {
        //public Task<User> GetUserByUsernameAndPassword(int id);
            User GetUserByUsernameAndPassword(string username, string password);
            string GetUserRole(string username);

        }
        public interface IInformationRL
        {
            public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
            public Task<int> GetTotalRecords(ReadAllInformationRequest request);
            public Task<List<ReadAllInformationRequest>> GetPaginatedRecords(ReadAllInformationRequest request, int pageNumber, int pageSize, string sortBy, string sortDirection);

            // public Task<ReadAllInformationResponse> ReadAllInformation(ReadAllInformationRequest request);
            public Task<UpdateAllInformationByIdResponse> UpdateAllInformationById(UpdateAllInformationByIdRequest request);
            public Task<DeleteInformationByIdResponse> DeleteInformationById(DeleteInformationByIdRequest request);
            public Task<GetInformationByIdResponse> GetInformationById(int id);

        }



    
}
