using MinhaApiCompleta.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetById(Guid id);

        Task<IEnumerable<TEntity>> GetAll();

        Task Include(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(Guid id);

        Task<int> SaveChanges();
    }
}