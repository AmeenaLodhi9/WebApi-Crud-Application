namespace webApiCrudApplication.CommonLayer.model
{
    public class AuthenticationDataResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
