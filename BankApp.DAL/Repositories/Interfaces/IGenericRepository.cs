using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BankApp.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity FindSingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity Find(int id);
        IEnumerable<TEntity> GetAll();
    }
}

