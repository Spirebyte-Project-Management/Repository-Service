using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibGit2Sharp;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Exceptions;
using Spirebyte.Services.Repositories.Application.PullRequests.Events;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands.Handler;

public class CreatePullRequestHandler : ICommandHandler<CreatePullRequest>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IPullRequestRequestStorage _pullRequestRequestStorage;
    private readonly IRepositoryRepository _repositoryRepository;

    public CreatePullRequestHandler(IRepositoryRepository repositoryRepository, IContextAccessor contextAccessor,
        IMessageBroker messageBroker, IPullRequestRepository pullRequestRepository,
        IPullRequestRequestStorage pullRequestRequestStorage, IProjectsApiHttpClient projectsApiHttpClient)
    {
        _repositoryRepository = repositoryRepository;
        _contextAccessor = contextAccessor;
        _messageBroker = messageBroker;
        _pullRequestRepository = pullRequestRepository;
        _pullRequestRequestStorage = pullRequestRequestStorage;
        _projectsApiHttpClient = projectsApiHttpClient;
    }

    public async Task HandleAsync(CreatePullRequest command, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        var userId = _contextAccessor.Context.GetUserId();
        
        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.CreatePullRequests, userId,
                repository.ProjectId)) throw new ActionNotAllowedException();

        var pullRequestCount = await _pullRequestRepository.GetPullRequestCountOfRepositoryAsync(command.RepositoryId);

        var creationTime = DateTime.Now;

        var newPullRequest = new PullRequest(pullRequestCount + 1, command.Name, command.Description, command.Status,
            new List<PullRequestAction>(), command.Head, command.Branch, userId, creationTime,
            creationTime);

        var descriptionComment = new PullRequestAction(creationTime, PullRequestActionType.Comment, command.Description,
            Array.Empty<string>(), userId);
        newPullRequest.AddAction(descriptionComment);

        await _pullRequestRepository.AddAsync(repository.Id, newPullRequest);

        _pullRequestRequestStorage.SetPullRequest(command.ReferenceId, newPullRequest);

        await _messageBroker.SendAsync(new PullRequestCreated(newPullRequest, repository.Id), cancellationToken);
    }
}