using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CQRS.Business.Repository
{
    public abstract class RepositoryBase<TEntity> : 
        IRepositoryBase<TEntity>, IDisposable where TEntity : class
    {
        protected DbContext _DbContext { get; set; }

        public RepositoryBase(DbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task Create(TEntity entity)
        {
            await _DbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _DbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await _DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByConditionAync(Expression<Func<TEntity, bool>> expression)
        {
            return await _DbContext.Set<TEntity>().Where(expression).ToListAsync();
        }

        public async Task<TEntity> FindEntityByConditionAync(Expression<Func<TEntity, bool>> expression)
        {
            return await _DbContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task CommitAsync()
        {
            await this._DbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _DbContext.Set<TEntity>().Update(entity);
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }
    }
}
