using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly DbSet<Account> _entities;
        public AccountRepository(BankContext context)
        {
            _entities = context.Accounts;
        }
        public void Add(Account entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Account> entities)
        {
            throw new NotImplementedException();
        }

        public Account Find(int id)
        {
            throw new NotImplementedException();
        }

        public Account FindSingleOrDefault(Expression<Func<Account, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> Get(Expression<Func<Account, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Account entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Account> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Account entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Account> entities)
        {
            throw new NotImplementedException();
        }
    }
}
