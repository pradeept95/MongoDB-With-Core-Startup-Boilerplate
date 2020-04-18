using AspNetCore.MongoDb.Repository.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.MongoDb.Repository
{
    public interface IMongoRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        IMongoCollection<TEntity> Collection { get; }

        int Count();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        bool Delete(Expression<Func<TEntity, bool>> predicate);
        bool Delete(ObjectId id);
        bool Delete(TEntity entity);
        bool Delete(TPrimaryKey id);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteAsync(ObjectId id);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(TPrimaryKey id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(ObjectId id);
        TEntity FirstOrDefault(TPrimaryKey id);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(ObjectId id);
        TEntity Get(ObjectId id);
        TEntity Get(TPrimaryKey id);
        IQueryable<TEntity> GetAll();
        List<TEntity> GetAllList();
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllListAsync();
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(ObjectId id);
        Task<TEntity> GetAsync(TPrimaryKey id);
        TEntity Insert(TEntity entity);
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);
        Task<TEntity> InsertAsync(TEntity entity);
        long LongCount();
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync();
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(ObjectId id, Func<TEntity, Task<TEntity>> updateAction);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}