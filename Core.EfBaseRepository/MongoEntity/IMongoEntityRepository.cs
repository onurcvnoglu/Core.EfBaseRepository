using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.EfBaseRepository.MongoEntity
{
    public interface IMongoEntityRepository<TEntity> where TEntity : class
    {
        public List<TEntity> GetAll(Expression<Func<TEntity, object>>? sortByDesc = null, int? page = null, int? limit = null);
        public TEntity GetById(string id);
        public List<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>>? sortByDesc = null, int? page = null, int? limit = null);
        public void Insert(TEntity document);
        public void InsertMany(List<TEntity> document);
        public void Update(string id, TEntity document);
        public void UpdateMany(string id, List<TEntity> document);
        public void Delete(string id);


        public Task<List<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(string id);
        public Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filterExpression);
        public Task InsertAsync(TEntity document);
        public Task InsertManyAsync(List<TEntity> document);
        public Task UpdateAsync(string id, TEntity document);
        public Task UpdateManyAsync(List<TEntity> document);
        public Task DeleteAsync(string id);

        public int TotalCount(Expression<Func<TEntity, bool>>? filterExpression);
    }
}
