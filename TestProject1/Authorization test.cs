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
    public class Authorization_test
    {
        private readonly Mock<IUserSL> _userSLMock;
        private readonly Mock<IInformationSL> _informationSLMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly CrudApplicationController _controller;
        public Authorization_test()
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
        [InlineData("Admin", true, "You are authorized as Admin!")]
        [InlineData("User", false, null)] // Non-admin user should not pass the authorization
        public void GetAdminData_RoleBasedAuthorizationTests(string role, bool isAuthorized, string expectedMessage)
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, role) // Passing role from the InlineData
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = _controller.GetAdminData();

            // Assert
            if (isAuthorized)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<AuthenticationDataResponse>(okResult.Value);

                Assert.True(response.IsSuccess);
                Assert.Equal(expectedMessage, response.Message);

                // Verify the logger was called
                //_mockLogger.Verify(logger => logger.Log(expectedMessage, null), Times.Once);
            }
            else
            {
                // For non-authorized users, it should return ForbidResult (or UnauthorizedResult if that's the logic)
                Assert.IsType<ForbidResult>(result);
                //_mockLogger.Verify(logger => logger.Log(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            }

        }
        [Theory]
        [InlineData("User", true, "You are authorized as User!")]
        [InlineData("Admin", false, null)] // Non-user roles should not pass the authorization
        public void GetUserData_RoleBasedAuthorizationTests(string role, bool isAuthorized, string expectedMessage)
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, role) // Passing role from InlineData
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = _controller.GetUserData(); // Ensure you're calling GetUserData

            // Assert
            if (isAuthorized)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsType<AuthenticationDataResponse>(okResult.Value); // Assuming your response type is AuthenticationDataResponse

                Assert.True(response.IsSuccess);
                Assert.Equal(expectedMessage, response.Message);

                // Verify the data
                Assert.NotNull(response.Data);
                var data = Assert.IsType<object[]>(response.Data); // Assuming Data is an object array
                Assert.Equal(2, data.Length); // Ensure data contains 2 elements

                // Verify the logger was called
                //_mockLogger.Verify(logger => logger.Log(expectedMessage, null), Times.Once);
            }
            else
            {
                // For non-authorized users, it should return ForbidResult
                Assert.IsType<ForbidResult>(result);

                // Verify the logger was not called
                //_mockLogger.Verify(logger => logger.Log(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
            }
        }
    }
}
