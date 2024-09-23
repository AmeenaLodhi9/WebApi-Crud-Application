using System.ComponentModel.DataAnnotations;

namespace webApiCrudApplication.CommonLayer.model
{

    public class GetInformationByIdRequest
    {
        public int UserID { get; set; }

        // Mark these as nullable if they can be null or left empty in certain cases
        public string UserName { get; set; }
        [Required]
        public string EmailId { get; set; }
        public string? MobileNumber { get; set; }

        public int Salary { get; set; }

        // Gender can also be nullable if it's optional
        public string? Gender { get; set; }

        // Assume IsActive is a required field, but consider defaulting to true or false if necessary
        public bool IsActive { get; set; }


    }
    public class GetInformationByIdResponse
    {
        // Success flag for the response
        public bool IsSuccess { get; set; }

        // Message field, remove the required keyword if you're using C# < 11
        public string Message { get; set; } = string.Empty; // Provide default value to avoid issues

        // Optionally, you can include the retrieved data here
        public GetInformationByIdRequest? Data { get; set; }  // This will hold the request data if needed
    }
}
