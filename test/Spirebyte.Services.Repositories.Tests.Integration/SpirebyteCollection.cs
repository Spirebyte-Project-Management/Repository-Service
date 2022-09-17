using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Xunit;

[assembly: CollectionBehavior(MaxParallelThreads = 1, DisableTestParallelization = true)]


namespace Spirebyte.Services.Repositories.Tests.Integration;

[CollectionDefinition(nameof(SpirebyteCollection), DisableParallelization = true)]
public class SpirebyteCollection : ICollectionFixture<DockerDbFixture>,
    ICollectionFixture<MongoDbFixture<ProjectDocument, string>>,
    ICollectionFixture<MongoDbFixture<RepositoryDocument, string>>
{
}