using System.Text.Json.Serialization;

namespace webApiCrudApplication.CommonLayer.model
{

    public class GetReadAllInformationRequest
    {

        public int? UserID { get; set; }
        public  string UserName { get; set; }
        public  string EmailId { get; set; }
        public  string MobileNumber { get; set; }
        public  int? Salary { get; set; }
        public  string Gender { get; set; }
        public  bool IsActive { get; set; }
        [JsonIgnore]
        public int? PageNumber { get; set; }  // Optional
        [JsonIgnore]
        public int? PageSize { get; set; }
        [JsonIgnore]
        public string SortBy { get; set; } // Field name to sort by
        [JsonIgnore]
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
