using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRS.Business.Repository
{
    public interface IRepositoryBase<TEntity> : IDisposable
    {
        Task<IEnumerable<TEntity>> FindAllAsync();
        Task<IEnumerable<TEntity>> FindByConditionAync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindEntityByConditionAync(Expression<Func<TEntity, bool>> expression);
        Task Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task CommitAsync();
    }
}
