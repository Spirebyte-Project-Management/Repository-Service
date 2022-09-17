using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.FileStorage.S3.Services;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Framework.Tests.Shared.Fixtures;
using Spirebyte.Framework.Tests.Shared.Infrastructure;
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
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration.Repository.Commands;

public class CreateRepositoryHandlerTests : TestBase
{
    private readonly IContextAccessor _contextAccessor;
    private readonly ICommandHandler<CreateRepository> _handler;
    private readonly TestMessageBroker _messageBroker;
    private readonly IS3Service _s3Service;

    private readonly IProjectRepository _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IRepositoryRepository _repositoryRepository;

    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public CreateRepositoryHandlerTests(MongoDbFixture<ProjectDocument, string> projectsFixture,
        MongoDbFixture<RepositoryDocument, string> repositoryFixture) : base(projectsFixture, repositoryFixture)
    {
        _projectRepository = new ProjectRepository(ProjectsMongoDbFixture);
        _repositoryRepository = new RepositoryRepository(RepositoriesMongoDbFixture);
        _messageBroker = new TestMessageBroker();

        _repositoryRequestStorage = Substitute.For<IRepositoryRequestStorage>();
        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _contextAccessor = Substitute.For<IContextAccessor>();
        _contextAccessor.Context.Returns(new Context("some-activity-id", "some-trace-id", "some-correlation-id", "some-message-id", "some-causation-id", Guid.NewGuid().ToString()));
        
        _s3Service = Substitute.For<IS3Service>();

        _handler = new CreateRepositoryHandler(_projectRepository, _repositoryRepository, _messageBroker,
            _repositoryRequestStorage, _s3Service, _projectsApiHttpClient, _contextAccessor);
    }

    [Fact]
    public async Task given_valid_command_create_repository_should_succeed()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        await _projectRepository.AddAsync(new Project(fakedRepository.ProjectId));

        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        var command =
            new CreateRepository(fakedRepository.Title, fakedRepository.Description, fakedRepository.ProjectId);

        var currentRepositories = await RepositoriesMongoDbFixture.FindAsync(r => r.ProjectId == fakedRepository.ProjectId);
        _repositoryRequestStorage.SetRepository(
            Arg.Do<Guid>(r => { r.Should().Be(command.ReferenceId); }),
            Arg.Do<Core.Entities.Repository>(r =>
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

        var repositories = await RepositoriesMongoDbFixture.FindAsync(r => r.Title == fakedRepository.Title);
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
}