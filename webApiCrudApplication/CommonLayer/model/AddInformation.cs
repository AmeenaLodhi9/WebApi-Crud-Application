using System.ComponentModel.DataAnnotations;

namespace webApiCrudApplication.CommonLayer.model
{
    using System.ComponentModel.DataAnnotations;

    public class AddInformationRequest
    {
        //[Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }

        //[Required(ErrorMessage = "EmailId is required")]
       // [RegularExpression("^[0-9a-zA-Z]+([._+-][0-9a-zA-Z]+)*@[0-9a-zA-Z]+\\.[a-zA-Z]{2,4}([.][a-zA-Z]{2,3})?$",
           // ErrorMessage = "Email ID is not valid")]
        public required string EmailId { get; set; }

        //[Required(ErrorMessage = "MobileNumber is required")]
        //[RegularExpression("([1-9]{1}[0-9]{9})$", ErrorMessage = "Mobile Number is not valid")]
        public required string MobileNumber { get; set; }

        //[Required(ErrorMessage = "Salary is required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please enter a Salary greater than 0")]
        public required int Salary { get; set; }

        //[Required(ErrorMessage = "Gender is required")]
        //[RegularExpression("^(?:m|M|male|Male|f|F|female|Female)$", ErrorMessage = "Gender is not valid")]
        public required string Gender { get; set; }
    }

    public class AddInformationResponse
    {
        public bool IsSuccess { get; set; }  // Corrected property name
        public required string Message { get; set; }  // Ensure this is set when creating an object
    }

}
