namespace webApiCrudApplication.CommonLayer.model
{
    public class AddInformationRequest
    {
        public required string UserName { get; set; }
        public required string EmailId { get; set; }
        public required string MobileNumber { get; set; }
        public required int Salary { get; set; }
        public required string Gender { get; set; }
    }
    public class AddInformationResponse
    {
        public bool IsSuccess { get; set; }  // Corrected property name
        public required string Message { get; set; }  // Ensure this is set when creating an object
    }

}
