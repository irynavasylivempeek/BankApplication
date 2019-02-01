using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        IEnumerable<Transaction> GetWithReceiver(Expression<Func<Transaction, bool>> predicate);
    }
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {

        public TransactionRepository(BankContext context) : base(context)
        {
        }

        public IEnumerable<Transaction> GetWithReceiver(Expression<Func<Transaction, bool>> predicate)
        {
            return _entities
                .Include(c => c.ReceiverAccount)
                .ThenInclude(c => c.User)
                .Where(predicate);
        }
    }
}
