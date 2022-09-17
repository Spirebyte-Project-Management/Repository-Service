using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.FileStorage.S3.Services;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Exceptions;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using Branch = Spirebyte.Services.Repositories.Core.Entities.Branch;
using Repository = LibGit2Sharp.Repository;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;

internal sealed class CreateRepositoryHandler : ICommandHandler<CreateRepository>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMessageBroker _messageBroker;
    private readonly IS3Service _s3Service;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryRequestStorage _repositoryRequestStorage;

    public CreateRepositoryHandler(IProjectRepository projectRepository, IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IRepositoryRequestStorage repositoryRequestStorage, IS3Service s3Service,
        IProjectsApiHttpClient projectsApiHttpClient, IContextAccessor contextAccessor)
    {
        _projectRepository = projectRepository;
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _repositoryRequestStorage = repositoryRequestStorage;
        _s3Service = s3Service;
        _projectsApiHttpClient = projectsApiHttpClient;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(CreateRepository command, CancellationToken cancellationToken = default)
    {
        if (!await _projectRepository.ExistsAsync(command.ProjectId))
            throw new ProjectNotFoundException(command.ProjectId);

        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.CreateRepositories,
                _contextAccessor.Context.GetUserId(),
                command.ProjectId)) throw new ActionNotAllowedException();

        var repositoryCount = await _repositoryRepository.GetRepositoryCountOfProjectAsync(command.ProjectId);
        var repositoryId = $"{command.ProjectId}-repository-{repositoryCount + 1}";

        var referenceId = Guid.Empty;
        var repoPath = RepoPathHelpers.GetCachePathForRepositoryId(repositoryId);
        Repository.Init(repoPath, true);

        RepoPathHelpers.UpdateRepoCacheReference(repositoryId, referenceId);

        var branches = new List<Branch>();
        var pullRequests = new List<PullRequest>();

        var repository = new Core.Entities.Repository(repositoryId, command.Title, command.Description,
            command.ProjectId, referenceId, branches, pullRequests, command.CreatedAt);
        await _repositoryRepository.AddAsync(repository);

        await _s3Service.UploadDirAsync(repoPath, $"{command.ProjectId}/{repositoryId}");

        await _messageBroker.SendAsync(new RepositoryCreated(repository), cancellationToken);

        _repositoryRequestStorage.SetRepository(command.ReferenceId, repository);
    }
}