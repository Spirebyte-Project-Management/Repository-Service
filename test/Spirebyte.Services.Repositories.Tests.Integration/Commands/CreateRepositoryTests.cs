using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.API;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Repositories.Tests.Shared.Factories;
using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration.Commands;

[Collection("Spirebyte collection")]
public class CreateRepositoryTests : IDisposable
{
    private const string Exchange = "Repositories";
    private readonly ICommandHandler<CreateRepository> _commandHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<RepositoryDocument, string> _repositoryMongoDbFixture;

    public CreateRepositoryTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _repositoryMongoDbFixture = new MongoDbFixture<RepositoryDocument, string>("Repositories");
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _commandHandler = factory.Services.GetRequiredService<ICommandHandler<CreateRepository>>();
    }

    public async void Dispose()
    {
        _repositoryMongoDbFixture.Dispose();
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task create_Repository_command_should_add_Repository_with_given_data_to_database()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectKey" + Guid.NewGuid();
        
        var expectedRepositoryKey = $"{projectId}-Repository-1";

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());


        var command = new CreateRepository(title, description, projectId);

        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        var repository = await _repositoryMongoDbFixture.GetAsync(expectedRepositoryKey);

        repository.Should().NotBeNull();
        repository.Id.Should().Be(expectedRepositoryKey);
        repository.Title.Should().Be(title);
        repository.Description.Should().Be(description);
        repository.ProjectId.Should().Be(projectId);
    }

    [Fact]
    public async void create_Repository_command_fails_when_project_does_not_exist()
    {
        var title = "Title";
        var description = "description";
        var projectId = "projectId" + Guid.NewGuid();

        var command = new CreateRepository(title, description, projectId);


        // Check if exception is thrown

        await _commandHandler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }
}