using BankApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankApp.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly BankContext _context;
        protected readonly DbSet<TEntity> _entities;

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public TEntity Find(int id)
        {
            return _entities.Find(id);
        }

        public TEntity FindSingleOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }
    }
}
