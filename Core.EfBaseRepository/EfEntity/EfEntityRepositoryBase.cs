using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.EfBaseRepository.EfEntity
{
    public class EfEntityRepositoryBase<TEntity, TContext>(TContext context) : IEntityRepository<TEntity> where TEntity : class where TContext : DbContext, new()

    {
        private readonly TContext _context = context;

        // Add a new entity to the database
        public TEntity Add(TEntity entity)
        {
            var addedEntity = _context.Add(entity);
            _context.SaveChanges();
            return addedEntity.Entity;
        }

        // Delete an entity from the database
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        // Get an entity by its ID
        public TEntity? GetById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        // Get a single entity based on a filter expression
        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return _context.Set<TEntity>().FirstOrDefault(filter);
        }

        // Get all entities with optional filtering, sorting, and pagination
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sortByDesc = null, int? page = null, int? limit = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (sortByDesc != null)
                query = sortByDesc(query);

            if (page.HasValue && limit.HasValue && page > 0 && limit > 0)
            {
                query = query.Skip((page.Value - 1) * limit.Value).Take(limit.Value);
            }

            return query;
        }

        // Update an existing entity
        public void Update(TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        // Update multiple entities
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
            _context.SaveChanges();
        }

        // Count the number of entities matching a filter
        public int Count(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().Count()
                : _context.Set<TEntity>().Count(filter);
        }
    }
}
