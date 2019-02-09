using BankApp.BLL;
using BankApp.Controllers;
using BankApp.DTO;
using BankApp.DTO.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neleus.LambdaCompare;

namespace BankApp.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userService;
        private Mock<HttpContext> _httpContext;
        private IConfiguration _config;

        private Login _user;
        private const int UserId = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new Login() { UserName = "UserName", Password = "Password" };

            _userService = new Mock<IUserService>();
            _httpContext = new Mock<HttpContext>();

            var userClaims = new Mock<ClaimsPrincipal>();
            userClaims.Setup(c => c.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));

            _httpContext.SetupGet(c => c.User).Returns(userClaims.Object);

            _config = Mock.Of<IConfiguration>(x =>
                x["Jwt:Issuer"] == "http://localhost:53874" &&
                x["Jwt:Key"] == "veryVerySecretKey" &&
                x["Jwt:ExpiryInMinutes"] == "30");
        }

        [TestMethod]
        public void Register()
        {
            _userService
                .Setup(c => c.Add(_user))
                .Returns(new LoginResult()
                {
                    Success = true,
                    User = new User()
                    {
                        UserId = 1,
                        UserName = "UserName",
                        Balance = 0
                    }
                });

            var userController = new UserController(_userService.Object, _config);
            var result = userController.Register(_user);
            Assert.IsTrue(result.Success && !String.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        public void Register_ReturnFailureWithoutToken_WhenUserNameExists()
        {
            _userService
                .Setup(c => c.Add(_user))
                .Returns(new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "There is a user with the same login"
                });

            var userController = new UserController(_userService.Object, _config);
            var result = userController.Register(_user);
            Assert.IsTrue(!result.Success && String.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        public void Login()
        {
            _userService
                .Setup(c => c.Login(It.IsAny<Login>()))
                .Returns(new LoginResult()
                {
                    Success = true,
                    User = new User()
                    {
                        UserId = 1,
                        UserName = "UserName",
                        Balance = 0
                    }
                });

            var userController = new UserController(_userService.Object, _config);
            var result = userController.Login(_user);
            Assert.IsTrue(result.Success && !String.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        public void Login_ReturnFailureWithoutToken_WhenLoginFailed()
        {
            _userService
                .Setup(c => c.Login(_user))
                .Returns(new LoginResult()
                {
                    Success = false,
                    ErrorMessage = "Wrong login"
                });

            var userController = new UserController(_userService.Object, _config);
            var result = userController.Login(_user);
            Assert.IsTrue(!result.Success && String.IsNullOrEmpty(result.Token));
        }

        [TestMethod]
        public void UserInfo()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(Mock.Of<User>());

            var userController = new UserController(_userService.Object, _config)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };
            var user = userController.UserInfo();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void UserInfo_ReturnNull_WhenUserDoesntExist()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns((User)null);

            var userController = new UserController(_userService.Object, _config)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };
            var user = userController.UserInfo();
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetAll()
        {
            var allUsers = new List<User>()
            {
                new User(){ UserId = 1, UserName = "username" },
                new User(){ UserId = 2, UserName = "username2" },
                new User(){ UserId = 3, UserName = "username3" }
            };

            _userService
                .Setup(c => c.GetAll())
                .Returns(allUsers);

            var userController = new UserController(_userService.Object, _config)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };
            var users = userController.GetAll().ToList();
            Assert.IsTrue(users.All(c => c.UserId != UserId) && (users.Count == allUsers.Count - 1));
        }
    }
}
