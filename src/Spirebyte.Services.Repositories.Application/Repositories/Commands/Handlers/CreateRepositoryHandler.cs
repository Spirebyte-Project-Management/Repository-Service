using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Partytitan.Convey.Minio.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using Branch = Spirebyte.Services.Repositories.Core.Entities.Branch;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;

// Simple wrapper
internal sealed class CreateRepositoryHandler : ICommandHandler<CreateRepository>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IMinioService _minioService;
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public CreateRepositoryHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IRepositoryRequestStorage repositoryRequestStorage, IMinioService minioService)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _repositoryRequestStorage = repositoryRequestStorage;
        _minioService = minioService;
    }

    public async Task HandleAsync(CreateRepository command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        var repositoryCount = await _repositoryRepository.GetRepositoryCountOfProjectAsync(command.ProjectId);
        var repositoryId = $"{command.ProjectId}-repository-{repositoryCount + 1}";

        var referenceId = Guid.Empty;
        var repoPath = RepoPathHelpers.GetCachePathForRepositoryId(repositoryId);
        Repository.Init(repoPath, true);

        RepoPathHelpers.UpdateRepoCacheReference(repositoryId, referenceId);

        var branches = new List<Branch>();

        var repository = new Core.Entities.Repository(repositoryId, command.Title, command.Description,
            command.ProjectId, referenceId, branches, command.CreatedAt);
        await _repositoryRepository.AddAsync(repository);

        await _minioService.UploadDirAsync(repoPath, $"{command.ProjectId}/{repositoryId}");

        await _messageBroker.PublishAsync(new RepositoryCreated(repository));

        _repositoryRequestStorage.SetRepository(command.ReferenceId, repository);
    }
}