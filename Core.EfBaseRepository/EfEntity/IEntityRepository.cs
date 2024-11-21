using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.EfBaseRepository.EfEntity
{
    public interface IEntityRepository<TEntity> where TEntity : class
    {
        TEntity? Get(Expression<Func<TEntity, bool>> filter);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity Add(TEntity entity);
        TEntity? GetById(object id);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sortByDesc = null, int? page = null, int? limit = null);
    }
}
