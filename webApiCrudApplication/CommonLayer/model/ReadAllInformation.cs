namespace webApiCrudApplication.CommonLayer.model
{

    public class GetReadAllInformation
    {

        public required int UserID { get; set; }
        public required string UserName { get; set; }
        public required string EmailId { get; set; }
        public required string MobileNumber { get; set; }
        public required int Salary { get; set; }
        public required string Gender { get; set; }
        public required bool IsActive { get; set; }

    }
    public class ReadAllInformationResponse
    {
        public bool IsSuccess { get; set; }  // Corrected property name
        public required string Message { get; set; }
        public List<GetReadAllInformation> readAllInformation { get; set; }
        public ReadAllInformationResponse()
        {
            readAllInformation = new List<GetReadAllInformation>();
        }

    }
}
