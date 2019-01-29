using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;
using BankApp.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankContext context) : base(context)
        {
        }
    }
}
