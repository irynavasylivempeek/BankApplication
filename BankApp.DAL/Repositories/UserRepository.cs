using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BankApp.DAL.Repositories.Interfaces;
using BankApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankApp.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly DbSet<User> _entities;
        public UserRepository(BankContext context)
        {
            _entities = context.Users;
        }
        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public User Find(int id)
        {
            throw new NotImplementedException();
        }

        public User FindSingleOrDefault(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }
    }
}
