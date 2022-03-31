using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Repositories.Tests.Integration;

[CollectionDefinition("Spirebyte collection", DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<DockerDbFixture>, ICollectionFixture<MongoDbFixture<ProjectDocument, string>>, ICollectionFixture<MongoDbFixture<RepositoryDocument, string>>
{
}