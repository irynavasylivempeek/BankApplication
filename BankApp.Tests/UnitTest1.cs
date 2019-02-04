using BankApp.BLL;
using BankApp.Controllers;
using BankApp.Domain.Enums;
using BankApp.DTO.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var transactionService = new Mock<ITransactionService>();
            var userService = new Mock<IUserService>();
            transactionService.Setup(c => c.MakeTransaction(It.IsAny<Transaction>()));
            userService.Setup(c => c.GetFullInfoById(It.IsAny<int>()));
            AccountController controller = new AccountController(transactionService.Object, userService.Object);

            var transactionResult = controller.Deposit(new Transaction()
            {
                SenderId = 1,
                Amount = 20,
                Type = TransactionType.Deposit
            });
            Assert.Equals(transactionResult.User.Balance, 90);
        }
    }
}
