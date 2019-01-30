using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BankApp.Domain;
using BankApp.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetIncludingAccount(Expression<Func<User, bool>> predicate);
    }
    public class UserRepository : GenericRepository<User>,  IUserRepository
    {
        public UserRepository(BankContext context) : base(context)
        {
            
        }

        public User GetIncludingAccount(Expression<Func<User, bool>> predicate)
        {
           
            return _entities.Include(c => c.Account)
                .ThenInclude(c => c.Transactions)
                .Include(c=>c.Account)
                .ThenInclude(c=>c.IncomingTransferTransactions)
                .ThenInclude(c=>c.Account)
                .SingleOrDefault(predicate);
        }
    }
}
