using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Convey.Types;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Spirebyte.Services.Repositories.Tests.Shared.Helpers;

namespace Spirebyte.Services.Repositories.Tests.Shared.Infrastructure.Mongo;

public class MongoDbTestRepository<TEntity, TKey> : IDisposable, IMongoRepository<TEntity, TKey>
    where TEntity : IIdentifiable<TKey>
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly string _databaseName;

    private bool _disposed;

    public MongoDbTestRepository(string collectionName, string settingsFile = null)
    {
        var options = OptionsHelper.GetOptions<MongoDbOptions>("mongo", settingsFile);
        _client = new MongoClient(options.ConnectionString);
        _databaseName = options.Database;
        _database = _client.GetDatabase(_databaseName);
        InitializeMongo();
        Collection = _database.GetCollection<TEntity>(collectionName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IMongoCollection<TEntity> Collection { get; }

    public Task<TEntity> GetAsync(TKey id)
    {
        return GetAsync(e => e.Id.Equals(id));
    }

    public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Collection.Find(predicate).SingleOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate)
    {
        return await Collection.Find(predicate).ToListAsync();
    }

    public Task<PagedResult<TEntity>> BrowseAsync<TQuery>(
        Expression<Func<TEntity, bool>> predicate,
        TQuery query)
        where TQuery : IPagedQuery
    {
        return Collection.AsQueryable().Where(predicate).PaginateAsync(query);
    }

    public Task AddAsync(TEntity entity)
    {
        return Collection.InsertOneAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        return UpdateAsync(entity, e => e.Id.Equals(entity.Id));
    }

    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
    {
        return Collection.ReplaceOneAsync(predicate, entity);
    }

    public Task DeleteAsync(TKey id)
    {
        return DeleteAsync(e => e.Id.Equals(id));
    }

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Collection.DeleteOneAsync(predicate);
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Collection.Find(predicate).AnyAsync();
    }

    private void InitializeMongo()
    {
        new MongoDbTestRepositoryInitializer(_database, null, new MongoDbOptions())
            .InitializeAsync().GetAwaiter().GetResult();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing) _client.DropDatabase(_databaseName);

        _disposed = true;
    }
}