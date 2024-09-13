namespace webApiCrudApplication.CommonLayer.model
{

    public class GetInformationByIdResponse
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public int Salary { get; set; }
        public string Gender { get; set; }
        public required bool IsActive { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
