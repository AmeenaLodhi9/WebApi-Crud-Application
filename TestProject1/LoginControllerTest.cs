using Moq;
using Xunit;
using webApiCrudApplication.Controllers;
using webApiCrudApplication.Logs;
using Microsoft.AspNetCore.Mvc;
using webApiCrudApplication.Services;
using webApiCrudApplication.CommonLayer.model;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TestProject1
{
    public class LoginControllerTest
    {
        private readonly Mock<IUserSL> _userSLMock;
        private readonly Mock<IInformationSL> _informationSLMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CrudApplicationController _controller;
        public LoginControllerTest()
        {
            // Arrange: Create mock objects for the dependencies
            _userSLMock = new Mock<IUserSL>();
            _informationSLMock = new Mock<IInformationSL>();
            _loggerMock = new Mock<ILogger>();


            // Inject the mocks into the controller
            _controller = new CrudApplicationController(
                _userSLMock.Object,
                _informationSLMock.Object,
                _loggerMock.Object
            );
        }
        [Theory]
        [InlineData("admin", "123", true)]    // Valid credentials (this should pass)
        [InlineData("admin", "wrongpassword", false)] // Invalid password (this should fail)
        [InlineData("nonexistent", "123", false)] // Invalid username (this should fail)
        public void Login_Test(string username, string password, bool expectedSuccess)
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            // Mock the response based on the username/password
            _userSLMock.Setup(x => x.Authenticate(It.Is<LoginRequest>(req => req.Username == "admin" && req.Password == "123")))
                       .Returns(new LoginResponse
                       {
                           IsSuccess = true,
                           Message = "Login Successfully",
                           Token = "valid-token"
                       });

            // Mock invalid username or password cases
            _userSLMock.Setup(x => x.Authenticate(It.Is<LoginRequest>(req => req.Username != "admin" || req.Password != "123")))
                       .Returns(new LoginResponse
                       {
                           IsSuccess = false,
                           Message = "Invalid username or password",
                           Token = null
                       });

            // Act
            var result = _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as dynamic;
            Assert.NotNull(response); // Check that response is not null
            Assert.Equal(expectedSuccess, response.IsSuccess);

            // Additional assertion for message verification
            if (expectedSuccess)
            {
                Assert.NotNull(response.Token);
                Assert.Equal("Login Successfully", response.Message);
            }
            else
            {
                Assert.Null(response.Token);
                Assert.Equal("Invalid username or password", response.Message);
            }
        }
    }
}
