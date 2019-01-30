using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using BankApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankApp.DAL
{
    public interface IUnitOfWork
    {
        ITransactionRepository Transactions { get; }
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        IDbContextTransaction BeginTransaction();
        int SaveChanges();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private ITransactionRepository _transactions;
        private IUserRepository _users;
        private IAccountRepository _accounts;
        private readonly BankContext _context;

        public UnitOfWork(BankContext context)
        {
            _context = context;
        }

        public ITransactionRepository Transactions => _transactions ?? (_transactions = new TransactionRepository(_context));

        public IUserRepository Users => _users ?? (_users = new UserRepository(_context));

        public IAccountRepository Accounts => _accounts ?? (_accounts = new AccountRepository(_context));

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
