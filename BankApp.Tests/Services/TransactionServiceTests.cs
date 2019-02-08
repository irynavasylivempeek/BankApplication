using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BankApp.BLL;
using BankApp.DAL.Repositories;
using BankApp.Domain;
using BankApp.Domain.Enums;
using BankApp.DTO.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests.Services
{
    [TestClass]
    public class TransactionServiceTests
    {
        [TestMethod]
        public void MakeTransaction_ThrowsDbUpdateConcurrencyException_WhenDbThrowsDbUpdateConcurrencyException()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var accountRepository = new Mock<IAccountRepository>();

            accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            accountRepository
                .Setup(c => c.BeginTransaction())
                .Returns(new Mock<IDbContextTransaction>().Object);

            accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new DbUpdateConcurrencyException("", new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var transactionService = new TransactionService(accountRepository.Object, transactionRepository.Object);

            Assert.ThrowsException<DbUpdateConcurrencyException>(() => transactionService.MakeTransaction(new DTO.Transactions.Transaction()));
           
        }

        [TestMethod]
        public void MakeTransaction_ThrowsExceptionWithMessage_WhenDbThrowsOtherExceptions()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            accountRepository
                .Setup(c => c.BeginTransaction())
                .Returns(new Mock<IDbContextTransaction>().Object);

            accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new Exception(""));

            var transactionService = new TransactionService(accountRepository.Object, transactionRepository.Object);
            try
            {
                transactionService.MakeTransaction(new DTO.Transactions.Transaction());
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Sorry, your transaction was canceled! Try again later");
            }
        }
    }
}
