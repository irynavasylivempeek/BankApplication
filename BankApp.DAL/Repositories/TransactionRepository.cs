using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;
using BankApp.Domain.Transactions;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        protected readonly DbSet<Transaction> _entities;
        public TransactionRepository(BankContext context)
        {
            _entities = context.Transactions;
        }
        public void Add(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Transaction> entities)
        {
            throw new NotImplementedException();
        }

        public Transaction Find(int id)
        { 
            throw new NotImplementedException();
        }

        public Transaction FindSingleOrDefault(Expression<Func<Transaction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> Get(Expression<Func<Transaction, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Transaction> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Transaction> entities)
        {
            throw new NotImplementedException();
        }
    }
}
