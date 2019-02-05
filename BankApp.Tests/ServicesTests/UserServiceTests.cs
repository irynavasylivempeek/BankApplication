using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.BLL;
using BankApp.DAL.Repositories;
using BankApp.DTO;
using BankApp.DTO.Users;
using BankApp.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using User = BankApp.Domain.User;

namespace BankApp.Tests.ServicesTests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void AddUser_ReturnsFalse_WhenExistsUserWithSameUserName()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(c=>c.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(new User());
            var userService = new UserService(userRepository.Object);
            var loginResult = userService.Add(It.IsAny<Login>());
            Assert.IsFalse(loginResult.Success);
        }

        [TestMethod]
        public void GetFullInfoById_ReturnsNull_WhenUserDoesntExist()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(c => c.GetWithTransactions(It.IsAny<Expression<Func<User, bool>>>())).Returns((User) null);
            var userService = new UserService(userRepository.Object);
            Assert.IsNull(userService.GetFullInfoById(It.IsAny<int>()));
        }

        [TestMethod]
        public void Login_ReturnsFalse_WhenUserDoesntExist()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns((User) null);
            var userService = new UserService(userRepository.Object);
            Assert.IsFalse(userService.Login(It.IsAny<Login>()).Success);
        }

        [TestMethod]
        public void Login_ReturnsFalse_WhenPasswordIsWrong()
        {
            var salt = SaltedHashGenerator.GenerateSalt();
            var user = new User
            {
                UserId = 1,
                UserName = "irynavasyliv",
                Password = SaltedHashGenerator.GenerateSaltedHash("24041998", salt),
                Salt = salt
            };
            var loginUser = new Login()
            {
                UserName = "irynavasyliv",
                Password = "24041999"
            };
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            var userService = new UserService(userRepository.Object);
            Assert.IsFalse(userService.Login(loginUser).Success);
        }

        [TestMethod]
        public void GetAll()
        {
            var allUsers = new List<User>()
            {
                new User()
                {
                    UserId = 1, UserName = "irynavasyliv"
                },
                new User()
                {
                    UserId = 2, UserName = "goryakdmytro",
                    
                }
            };
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
        }
    }
}
