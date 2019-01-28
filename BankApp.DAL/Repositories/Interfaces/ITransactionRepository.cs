using System;
using System.Collections.Generic;
using System.Text;
using BankApp.Domain.Transactions;

namespace BankApp.DAL.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
    }
}
