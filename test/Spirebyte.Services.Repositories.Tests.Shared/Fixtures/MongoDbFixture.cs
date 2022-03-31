#nullable enable
using System;
using Convey.Types;
using Spirebyte.Services.Repositories.Tests.Shared.Infrastructure;

namespace Spirebyte.Services.Repositories.Tests.Shared.Fixtures;

public class MongoDbFixture<TEntity, TKey> : MongoDbTestRepository<TEntity, TKey> where TEntity : IIdentifiable<TKey>
{
    public MongoDbFixture() : base(Guid.NewGuid().ToString())
    {
    }
}