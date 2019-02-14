using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using BankApp.BLL;
using BankApp.Controllers;
using BankApp.DTO.Transactions;
using BankApp.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests.BankApp.Tests
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
            userClaims
                .Setup(c => c.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, UserId.ToString()));

            _httpContext
                .SetupGet(c => c.User)
                .Returns(userClaims.Object);
        }

        // the same for Withdraw and Transfer
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

            _transactionService.Verify(c=>c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        // the same for Withdraw and Transfer
        [TestMethod]
        public void Deposit_ReturnFailure_WhenUserNotFound()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns((UserDetails)null);

            var transaction = new Transaction() { Amount = 10 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Deposit(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Sender was not found");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        //the same for Transfer
        [TestMethod]
        public void Withdraw_ReturnFailure_WhenLackOfMoney()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(new UserDetails{ Balance = 5 });

            var transaction = new Transaction() { Amount = 10 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Withdraw(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Lack of money to make transaction");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        [TestMethod]
        public void Transfer_ReturnFailure_WhenReceiverIsNull()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(new UserDetails { Balance = 10 });

            var transaction = new Transaction() { Amount = 10 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Transfer(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Receiver was not found");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        [TestMethod]
        public void Transfer_ReturnFailure_WhenReceiverDoesntExist()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(new UserDetails { Balance = 10 });

            _userService
                .Setup(c => c.Exists(It.IsAny<int>()))
                .Returns(false);

            var transaction = new Transaction() { Amount = 10, ReceiverId = 3 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Transfer(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Receiver was not found");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        [TestMethod]
        public void Transfer_ReturnFailure_WhenSenderAndReceiverAreSame()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(new UserDetails { UserId = UserId, Balance = 10 });

            var transaction = new Transaction() { Amount = 10, ReceiverId = UserId };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Transfer(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Sender and receiver cannot be the same account");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Never);
        }

        [TestMethod]
        public void Deposit_ReturnFailure_WhenThrowsDbUpdateConcurrencyException()
        {
            _userService
                .Setup(c => c.GetFullInfoById(It.IsAny<int>()))
                .Returns(new UserDetails { UserId = 1, Balance = 10 });

            _userService
                .Setup(c => c.Exists(It.IsAny<int>()))
                .Returns(true);

            _transactionService
                .Setup(c => c.MakeTransaction(It.IsAny<Transaction>()))
                .Throws(new Exception("Sorry, your transaction was canceled! Try again later"));

            var transaction = new Transaction() { Amount = 10, ReceiverId = 3 };
            var accountController = new AccountController(_transactionService.Object, _userService.Object)
            {
                ControllerContext = { HttpContext = _httpContext.Object }
            };

            var result = accountController.Transfer(transaction);
            Assert.IsTrue(!result.Success && result.ErrorMessage == "Sorry, your transaction was canceled! Try again later");

            _transactionService.Verify(c => c.MakeTransaction(It.IsAny<Transaction>()), Times.Once);
        }
    }
}
