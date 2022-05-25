using System;
using System.Threading.Tasks;
using Bogus;
using Convey.CQRS.Commands;
using FluentAssertions;
using NSubstitute;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Repositories;
using Spirebyte.Services.Repositories.Tests.Shared;
using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Spirebyte.Shared.Contexts.Interfaces;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration.Repository.Commands;

[Collection("Spirebyte collection")]
public class CreateRepositoryHandlerTests : IDisposable
{
    private readonly ICommandHandler<CreateRepository> _handler;
    private readonly TestMessageBroker _messageBroker;
    private readonly IMinioService _minioService;

    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly MongoDbFixture<ProjectDocument, string> _projectsFixture;
    private readonly MongoDbFixture<RepositoryDocument, string> _repositoryFixture;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IAppContext _appContext;

    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public CreateRepositoryHandlerTests(MongoDbFixture<ProjectDocument, string> projectsFixture, MongoDbFixture<RepositoryDocument, string> repositoryFixture)
    {
        _projectsFixture = projectsFixture;
        _repositoryFixture = repositoryFixture;
        
        _projectRepository = new ProjectRepository(_projectsFixture);
        _repositoryRepository = new RepositoryRepository(_repositoryFixture);
        _messageBroker = new TestMessageBroker();
        
        _repositoryRequestStorage = Substitute.For<IRepositoryRequestStorage>();
        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _appContext = Substitute.For<IAppContext>();
        _minioService = Substitute.For<IMinioService>();
        
        _handler = new CreateRepositoryHandler(_projectRepository, _repositoryRepository, _messageBroker,
            _repositoryRequestStorage, _minioService, _projectsApiHttpClient, _appContext);
    }

    [Fact]
    public async Task given_valid_command_create_repository_should_succeed()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        await _projectRepository.AddAsync(new Project(fakedRepository.ProjectId));

        var command =
            new CreateRepository(fakedRepository.Title, fakedRepository.Description, fakedRepository.ProjectId);

        var currentRepositories = await _repositoryFixture.FindAsync(r => r.ProjectId == fakedRepository.ProjectId);
        _repositoryRequestStorage.SetRepository(
            Arg.Do<Guid>(r => { r.Should().Be(command.ReferenceId); }),
            Arg.Do<Repositories.Core.Entities.Repository>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be($"{fakedRepository.ProjectId}-repository-{currentRepositories.Count + 1}");
                r.Title.Should().Be(fakedRepository.Title);
                r.Description.Should().Be(fakedRepository.Description);
                r.ProjectId.Should().Be(fakedRepository.ProjectId);
                r.Branches.Should().NotBeNull();
                r.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            })
        );

        await _handler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();
        
        _messageBroker.Events.Should().NotBeEmpty();
        _messageBroker.Events.Count.Should().Be(1);
        var @event = _messageBroker.Events[0];
        @event.Should().BeOfType<RepositoryCreated>();

        var repositories = await _repositoryFixture.FindAsync(r => r.Title == fakedRepository.Title);
        repositories.Should().NotBeEmpty();
    }

    [Fact]
    public async Task create_repository_should_fail_when_project_does_not_exist()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var command =
            new CreateRepository(fakedRepository.Title, fakedRepository.Description, fakedRepository.ProjectId);
        
        await _handler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }

    public void Dispose()
    {
        _projectsFixture.Dispose();
        _repositoryFixture.Dispose();
    }
}