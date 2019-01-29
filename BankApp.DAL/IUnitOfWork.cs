using System;
using System.Collections.Generic;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;

namespace BankApp.DAL
{
    public interface IUnitOfWork
    {
        ITransactionRepository Transactions { get; }
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        int SaveChanges();
    }
}
