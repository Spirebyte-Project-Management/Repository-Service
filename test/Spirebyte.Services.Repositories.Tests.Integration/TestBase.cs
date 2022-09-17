using System;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration;

[Collection(nameof(SpirebyteCollection))]
public class TestBase : IDisposable
{
    protected readonly MongoDbFixture<ProjectDocument, string> ProjectsMongoDbFixture;
    protected readonly MongoDbFixture<RepositoryDocument, string> RepositoriesMongoDbFixture;

    public TestBase(
        MongoDbFixture<ProjectDocument, string> projectsMongoDbFixture,
        MongoDbFixture<RepositoryDocument, string> repositoriesMongoDbFixture)
    {
        ProjectsMongoDbFixture = projectsMongoDbFixture;
        RepositoriesMongoDbFixture = repositoriesMongoDbFixture;
    }
    
    public void Dispose()
    {
        ProjectsMongoDbFixture.Dispose();
        RepositoriesMongoDbFixture.Dispose();
    }
}