namespace webApiCrudApplication.CommonLayer.model
{
    public class DeleteInformationByIdRequest
    {
        public required int UserId { get; set; }    
    }
    public class DeleteInformationByIdResponse
    {
        public bool IsSuccess { get; set; }   
        public required string Message {  get; set; } 
    }
}
