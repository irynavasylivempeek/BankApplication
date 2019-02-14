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
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Transaction = BankApp.Domain.Transaction;

namespace BankApp.Tests.BankApp.BLL.Tests
{
    [TestClass]
    public class TransactionServiceTests
    {
        private Mock<ITransactionRepository> _transactionRepository;
        private Mock<IAccountRepository> _accountRepository;
        private IConfiguration _config;
        private Mock<IDbContextTransaction> _dbContextTransaction;
        private int _attemptsCount = 5;

        [TestInitialize]
        public void TestInitialize()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _dbContextTransaction = new Mock<IDbContextTransaction>();
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository
                .Setup(c => c.BeginTransaction())
                .Returns(_dbContextTransaction.Object);

            _config = Mock.Of<IConfiguration>(c => c["ConcurrencyRetryAttempts"] == _attemptsCount.ToString());
        }

        [TestMethod]
        public void MakeTransaction()
        {
            _accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            var transactionService = new TransactionService(_accountRepository.Object, _transactionRepository.Object, _config);
            transactionService.MakeTransaction(Mock.Of<DTO.Transactions.Transaction>());

            _dbContextTransaction.Verify(c => c.Commit(), Times.Once);
            _transactionRepository.Verify(c => c.Add(It.IsAny<Transaction>()), Times.Once);
            _accountRepository.Verify(c => c.Update(It.IsAny<Account>()), Times.Once);
            _accountRepository.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void MakeTransaction_ThrowsDbUpdateConcurrencyException_WhenDbThrowsDbUpdateConcurrencyException()
        {
            _accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            _accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new DbUpdateConcurrencyException("", new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() }));

            var transactionService = new TransactionService(_accountRepository.Object, _transactionRepository.Object, _config);

            Assert.ThrowsException<Exception>(() => transactionService.MakeTransaction(Mock.Of<DTO.Transactions.Transaction>()), "Sorry, your transaction was canceled! Try again later");

            _dbContextTransaction.Verify(c=>c.Rollback(), Times.Exactly(_attemptsCount + 1));
            _transactionRepository.Verify(c=>c.Add(It.IsAny<Transaction>()), Times.Exactly(_attemptsCount + 1));
            _accountRepository.Verify(c => c.Update(It.IsAny<Account>()), Times.Exactly(_attemptsCount + 1));
            _accountRepository.Verify(c => c.SaveChanges(), Times.Exactly(_attemptsCount + 1));
        }

        [TestMethod]
        public void MakeTransaction_ThrowsExceptionWithMessage_WhenDbThrowsOtherExceptions()
        {
            _accountRepository
                .Setup(c => c.SingleOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
                .Returns(new Account() { AccountId = 1, UserId = 1, Balance = 10 });

            _accountRepository
                .Setup(c => c.SaveChanges())
                .Throws(new Exception(""));

            var transactionService = new TransactionService(_accountRepository.Object, _transactionRepository.Object, _config);
            Assert.ThrowsException<Exception>(()=> transactionService.MakeTransaction(new DTO.Transactions.Transaction()), "Sorry, your transaction was canceled! Try again later");

            _dbContextTransaction.Verify(c => c.Rollback(), Times.Once);
            _transactionRepository.Verify(c => c.Add(It.IsAny<Transaction>()), Times.Once);
            _accountRepository.Verify(c => c.Update(It.IsAny<Account>()), Times.Once);
            _accountRepository.Verify(c => c.SaveChanges(), Times.Once);
        }

    }
}
