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
        private Mock<ITransactionRepository> _transactionRepository;
        private Mock<IAccountRepository> _accountRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _accountRepository = new Mock<IAccountRepository>();
        }

        [TestMethod]
        public void MakeTransaction_ThrowsDbUpdateConcurrencyException_WhenDbThrowsDbUpdateConcurrencyException()
        {

            _accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            _accountRepository
                .Setup(c => c.BeginTransaction())
                .Returns(new Mock<IDbContextTransaction>().Object);

            _accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new DbUpdateConcurrencyException("", new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var transactionService = new TransactionService(_accountRepository.Object, _transactionRepository.Object);
            Assert.ThrowsException<DbUpdateConcurrencyException>(() => transactionService.MakeTransaction(new DTO.Transactions.Transaction()));   
        }

        [TestMethod]
        public void MakeTransaction_ThrowsExceptionWithMessage_WhenDbThrowsOtherExceptions()
        {
            _accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            _accountRepository
                .Setup(c => c.BeginTransaction())
                .Returns(new Mock<IDbContextTransaction>().Object);

            _accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new Exception(""));

            var transactionService = new TransactionService(_accountRepository.Object, _transactionRepository.Object);
            Assert.ThrowsException<Exception>(()=> transactionService.MakeTransaction(new DTO.Transactions.Transaction()), "Sorry, your transaction was canceled! Try again later"); 
        }
    }
}
