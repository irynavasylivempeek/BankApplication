using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using BankApp.BLL;
using BankApp.Controllers;
using BankApp.DTO.Transactions;
using BankApp.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<HttpContext> _httpContext;
        private Mock<IUserService> _userService;
        private Mock<ITransactionService> _transactionService;
        private const int UserId = 1;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpContext = new Mock<HttpContext>();
            _userService = new Mock<IUserService>();
            _transactionService = new Mock<ITransactionService>();

            var userClaims = new Mock<ClaimsPrincipal>();
            userClaims.Setup(c => c.FindFirst(It.IsAny<string>()))
                      .Returns(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));

            _httpContext.SetupGet(c => c.User).Returns(userClaims.Object);
        }

        [TestMethod]
        public void Deposit_ReturnFailure_WhenAmountIsLessThanZero()
        {
            var transaction = new Transaction() { Amount = -10 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };
            var result = accountController.Deposit(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Amount has to be positive number");
        }
    }
}
