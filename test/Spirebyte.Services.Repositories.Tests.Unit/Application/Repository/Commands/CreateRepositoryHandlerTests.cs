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
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Spirebyte.Shared.Contexts.Interfaces;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Application.Repository.Commands;

public class CreateRepositoryHandlerTests
{
    private readonly ICommandHandler<CreateRepository> _handler;
    private readonly IMessageBroker _messageBroker;
    private readonly IMinioService _minioService;

    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryRequestStorage _repositoryRequestStorage;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IAppContext _appContext;

    public CreateRepositoryHandlerTests()
    {
        _projectRepository = Substitute.For<IProjectRepository>();
        _repositoryRepository = Substitute.For<IRepositoryRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _repositoryRequestStorage = Substitute.For<IRepositoryRequestStorage>();
        _minioService = Substitute.For<IMinioService>();
        _projectsApiHttpClient = Substitute.For<IProjectsApiHttpClient>();
        _appContext = Substitute.For<IAppContext>();
        _handler = new CreateRepositoryHandler(_projectRepository, _repositoryRepository, _messageBroker,
            _repositoryRequestStorage, _minioService, _projectsApiHttpClient, _appContext);
    }

    [Fact]
    public async Task given_valid_command_create_repository_should_succeed()
    {
        var faker = new Faker();
        var repoCount = faker.Random.Number(0, 10000);
        var fakedRepository = RepositoryFaker.Instance.Generate();

        _projectRepository.ExistsAsync(fakedRepository.ProjectId).Returns(true);
        _repositoryRepository.GetRepositoryCountOfProjectAsync(fakedRepository.ProjectId).Returns(repoCount);
        _projectsApiHttpClient.HasPermission(default, default, default).ReturnsForAnyArgs(true);

        var command =
            new CreateRepository(fakedRepository.Title, fakedRepository.Description, fakedRepository.ProjectId);

        await _repositoryRepository
            .AddAsync(Arg.Do<Repositories.Core.Entities.Repository>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be($"{fakedRepository.ProjectId}-repository-{repoCount + 1}");
                r.Title.Should().Be(fakedRepository.Title);
                r.Description.Should().Be(fakedRepository.Description);
                r.ProjectId.Should().Be(fakedRepository.ProjectId);
                r.Branches.Should().NotBeNull();
                r.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            }));

        await _messageBroker
            .PublishAsync(Arg.Do<RepositoryCreated>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be($"{fakedRepository.ProjectId}-repository-{repoCount + 1}");
                r.Title.Should().Be(fakedRepository.Title);
                r.Description.Should().Be(fakedRepository.Description);
                r.ProjectId.Should().Be(fakedRepository.ProjectId);
                r.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            }));

        _repositoryRequestStorage.SetRepository(
            Arg.Do<Guid>(r => { r.Should().Be(command.ReferenceId); }),
            Arg.Do<Repositories.Core.Entities.Repository>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be($"{fakedRepository.ProjectId}-repository-{repoCount + 1}");
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