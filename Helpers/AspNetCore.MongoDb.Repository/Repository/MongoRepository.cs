using AspNetCore.MongoDb.Repository.Configuration.MongoDb;
using AspNetCore.MongoDb.Repository.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.MongoDb.Repository
{
    /// <summary>
    /// Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class MongoRepository<TEntity, TPrimaryKey> : IMongoRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IMongoDatabase _database = null;

        public MongoRepository(IOptions<MongoDbSettings> mongoConfiguration)
        {

            var client = new MongoClient(mongoConfiguration.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(mongoConfiguration.Value.Database);
        }

        public virtual IMongoCollection<TEntity> Collection
        {
            get
            {
                return _database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return Collection.Find(_ => true).ToList().AsQueryable();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public TEntity Get(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            var entity = Collection.Find(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return (TEntity)entity;
        }

        public async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            var entity = await Collection.FindAsync(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return await entity.FirstOrDefaultAsync();
        }

        public TEntity FirstOrDefault(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            return (TEntity)Collection.Find<TEntity>(filter).FirstOrDefault();
        }

        public TEntity Insert(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity.Id;
        }

        public TEntity Update(TEntity entity)
        {
            ReplaceOneResult actionResult = Collection.ReplaceOne(n => n.Id.Equals(entity.Id)
                                            , entity
                                            , new UpdateOptions { IsUpsert = true });
            if (!(actionResult.IsAcknowledged
                 && actionResult.ModifiedCount > 0))
            {
                throw new System.Exception("Unalbe to update entity.");
            }
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ReplaceOneResult actionResult = await Collection.ReplaceOneAsync(n => n.Id.Equals(entity.Id)
                                           , entity
                                           , new UpdateOptions { IsUpsert = true });
            if (!(actionResult.IsAcknowledged
                 && actionResult.ModifiedCount > 0))
            {
                throw new System.Exception("Unalbe to update entity.");
            }
            return entity;
        }

        public bool Delete(TEntity entity)
        {
            return Delete(entity.Id);
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            return await DeleteAsync(entity.Id);
        }

        public bool Delete(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            var result = Collection.DeleteOne(filter);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("There is no such an entity with given key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync(TPrimaryKey id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id, id);
            var result = await Collection.DeleteOneAsync(filter);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("There is no such an entity with given key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return result.IsAcknowledged;
        }

        public Task<TEntity> UpdateAsync(ObjectId id, Func<TEntity, Task<TEntity>> updateAction)
        {

            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var entity = Collection.Find(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return updateAction((TEntity)entity);
        }

        public int Count()
        {
            return (int)Collection.Find(_ => true).CountDocuments();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return (int)Collection.Find(predicate)
                .CountDocuments();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (int)(await Collection.Find(predicate)
                .CountDocumentsAsync());
        }

        public async Task<int> CountAsync()
        {
            return (int)(await Collection.Find(_ => true)
                .CountDocumentsAsync());
        }

        public bool Delete(ObjectId id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var result = Collection.DeleteOne(filter);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("There is no such an entity with given key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return result.IsAcknowledged;
        }

        public bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
           var result = Collection.DeleteMany(predicate);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("Item not found to delete.");
            }
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var result = await Collection.DeleteOneAsync(filter);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("There is no such an entity with given key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return result.IsAcknowledged;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await Collection.DeleteManyAsync(predicate);
            if (result.DeletedCount == 0)
            {
                throw new System.Exception("Item not found to delete.");
            }
            return result.IsAcknowledged;
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = Collection.Find(predicate);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key.");
            }
            return entity.FirstOrDefault();
        }

        public TEntity FirstOrDefault(ObjectId id)
        {

            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var entity = Collection.Find(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return entity.FirstOrDefault();

        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await Collection.FindAsync(predicate);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key.");
            }
            return await entity.FirstOrDefaultAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(ObjectId id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var entity = await Collection.FindAsync(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key.");
            }
            return await entity.FirstOrDefaultAsync();
        }

        public TEntity Get(ObjectId id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var entity = Collection.Find(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return (TEntity)entity;
        }

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = Collection.Find(predicate);
            return entity.ToList();
        }

        public List<TEntity> GetAllList()
        {
            var entity = Collection.Find(_ => true);
            return entity.ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(_ => true).ToListAsync();
        }

        public async Task<TEntity> GetAsync(ObjectId id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(m => m.Id.ToString(), id.ToString());
            var entity = await Collection.FindAsync(filter);
            if (entity == null)
            {
                throw new System.Exception("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }

            return (TEntity)entity;
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Collection.Find(predicate)
                 .CountDocuments();
        }

        public long LongCount()
        {
            return Collection.Find(_ => true)
                .CountDocuments();
        }

        public async Task<long> LongCountAsync()
        {
            return await Collection.Find(_ => true)
                 .CountDocumentsAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate)
                 .CountDocumentsAsync();
        }


    }
}
