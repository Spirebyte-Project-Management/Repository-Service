using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.API;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Queries;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Repositories.Tests.Shared.Factories;
using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration.Queries;

[Collection("Spirebyte collection")]
public class GetRepositoriesTests : IDisposable
{
    private const string Exchange = "Repositories";
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly IQueryHandler<GetRepositories, IEnumerable<RepositoryDto>> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<RepositoryDocument, string> _repositoryMongoDbFixture;

    public GetRepositoriesTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _repositoryMongoDbFixture = new MongoDbFixture<RepositoryDocument, string>("Repositories");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler =
            factory.Services.GetRequiredService<IQueryHandler<GetRepositories, IEnumerable<RepositoryDto>>>();
    }

    public async void Dispose()
    {
        _repositoryMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_Repositories_query_succeeds_when_a_Repository_exists()
    {
        var repositoryId = "RepositoryKey" + Guid.NewGuid();
        var repository2Id = "Repository2Key" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;
        
        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var repository = new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);
        var repository2 = new Repository(repository2Id, title, description, projectId, new List<Branch>(), createdAt);

        await _repositoryMongoDbFixture.InsertAsync(repository.AsDocument());
        await _repositoryMongoDbFixture.InsertAsync(repository2.AsDocument());


        var query = new GetRepositories(projectId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        var repositoryDtos = result as RepositoryDto[] ?? result.ToArray();
        repositoryDtos.Should().Contain(i => i.Id == repositoryId);
        repositoryDtos.Should().Contain(i => i.Id == repository2Id);
    }

    [Fact]
    public async Task get_Repositories_query_should_return_empty_when_no_Repositories_exist()
    {
        var query = new GetRepositories();

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeEmpty();
    }
}