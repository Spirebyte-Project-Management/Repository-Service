using System;
using System.Collections.Generic;
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
public class GetRepositoryTests : IDisposable
{
    private const string Exchange = "Repositories";
    private readonly IQueryHandler<GetRepository, RepositoryDto> _queryHandler;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<RepositoryDocument, string> _repositoryMongoDbFixture;

    public GetRepositoryTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _repositoryMongoDbFixture = new MongoDbFixture<RepositoryDocument, string>("Repositories");
        factory.Server.AllowSynchronousIO = true;
        _queryHandler = factory.Services.GetRequiredService<IQueryHandler<GetRepository, RepositoryDto>>();
    }

    public async void Dispose()
    {
        _repositoryMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task get_Repository_query_succeeds_when_Repository_exists()
    {
        var repositoryId = "RepositoryKey" + Guid.NewGuid();
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        var createdAt = DateTime.Now;

        var repository = new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);

        await _repositoryMongoDbFixture.InsertAsync(repository.AsDocument());


        var query = new GetRepository(repositoryId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().NotBeNull();
        result.Id.Should().Be(repositoryId);
        result.Title.Should().Be(title);
        result.Description.Should().Be(description);
    }

    [Fact]
    public async Task get_Repository_query_should_return_null_when_Repository_does_not_exist()
    {
        var repositoryId = "notExistingRepositoryKey";

        var query = new GetRepository(repositoryId);

        // Check if exception is thrown

        var requestResult = _queryHandler
            .Awaiting(c => c.HandleAsync(query));

        await requestResult.Should().NotThrowAsync();

        var result = await requestResult();

        result.Should().BeNull();
    }
}