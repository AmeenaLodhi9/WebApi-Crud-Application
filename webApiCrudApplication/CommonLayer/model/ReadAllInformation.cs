namespace webApiCrudApplication.CommonLayer.model
{

    public class GetReadAllInformationRequest
    {

        public int? UserID { get; set; }
        public  string UserName { get; set; }
        public  string EmailId { get; set; }
        public  string MobileNumber { get; set; }
        public  int Salary { get; set; }
        public  string Gender { get; set; }
        public  bool IsActive { get; set; }
        public int? PageNumber { get; set; }  // Optional
        public int? PageSize { get; set; }
        public string SortBy { get; set; } // Field name to sort by
        public string SortDirection { get; set; } // 'asc' or 'desc'

    }
    public class ReadAllInformationResponse
    {
        public bool IsSuccess { get; set; }  // Corrected property name
        public required string Message { get; set; }
        public List<GetReadAllInformationRequest> readAllInformation { get; set; }
        public ReadAllInformationResponse()
        {
            readAllInformation = new List<GetReadAllInformationRequest>();
        }

    }
}
