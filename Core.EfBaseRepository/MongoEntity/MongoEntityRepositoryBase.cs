using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.EfBaseRepository.MongoEntity
{
    public class MongoEntityRepositoryBase<TEntity> : IMongoEntityRepository<TEntity> where TEntity : class
    {
        private IMongoDatabase _database;
        private IMongoCollection<TEntity> _collection;
        public MongoEntityRepositoryBase(MongodOptions options)
        {
            var client = new MongoClient(options.ConnectionString);
            _database = client.GetDatabase(options.DatabaseName);
            _collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, object>>? sortByDesc = null, int? page = null, int? limit = null)
        {
            page = page == null || page == 0 ? 1 : page;
            limit = limit == null ? 0 : limit;
            var list = _collection.Find(new BsonDocument());
            var result = list.Skip((page - 1) * limit).Limit(limit);
            return sortByDesc == null ? result.ToList() : result.SortByDescending(sortByDesc).ToList();
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            var result = await _collection.FindAsync(new BsonDocument());
            return await result.ToListAsync();
        }
        public TEntity GetById(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            var result = _collection.Find(filter).FirstOrDefault();
            return result;
        }
        public async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            var result = await _collection.FindAsync(filter);
            return await result.FirstOrDefaultAsync();
        }
        public List<TEntity> GetByFilter(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, object>>? sortByDesc = null, int? page = null, int? limit = null)
        {
            page = page == null || page == 0 ? 1 : page;
            limit = limit == null ? 0 : limit;
            var result = _collection.Find(filterExpression).Skip((page - 1) * limit).Limit(limit);
            return sortByDesc == null ? result.ToList() : result.SortByDescending(sortByDesc).ToList();
        }
        public async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            var result = await _collection.FindAsync(filterExpression);
            return await result.ToListAsync();
        }
        public void Insert(TEntity document)
        {
            _collection.InsertOne(document);
        }
        public async Task InsertAsync(TEntity document)
        {
            await _collection.InsertOneAsync(document);
        }
        public void InsertMany(List<TEntity> document)
        {
            foreach (var item in document)
            {
                _collection.InsertOne(item);
            }
        }
        public async Task InsertManyAsync(List<TEntity> document)
        {
            await Parallel.ForEachAsync(document, async (item, ct) =>
            {
                await _collection.InsertOneAsync(item);
            });
        }
        public void Update(string id, TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            _collection.ReplaceOne(filter, document);
        }
        public async Task UpdateAsync(string id, TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.ReplaceOneAsync(filter, document);
        }
        public void UpdateMany(string id, List<TEntity> document)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            foreach (var item in document)
            {
                _collection.ReplaceOne(filter, item);
            }

        }
        public async Task UpdateManyAsync(List<TEntity> document)
        {
            await Parallel.ForEachAsync(document, async (item, ct) =>
            {

                var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(((dynamic)item)._id));
                await _collection.ReplaceOneAsync(filter, item);
            });
        }
        public void Delete(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            _collection.DeleteOne(filter);
        }
        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
            await _collection.DeleteOneAsync(filter);
        }
        public int TotalCount(Expression<Func<TEntity, bool>>? filterExpression)
        {
            var result = filterExpression == null ? _collection.Find(new BsonDocument()).CountDocuments() : _collection.Find(filterExpression).CountDocuments();
            return (int)result;
        }
    }

}
