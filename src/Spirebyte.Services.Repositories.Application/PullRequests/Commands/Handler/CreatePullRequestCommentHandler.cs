﻿using System;
using System.Threading;
using System.Threading.Tasks;
using LibGit2Sharp;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.PullRequests.Events;
using Spirebyte.Services.Repositories.Application.PullRequests.Exceptions;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands.Handler;

public class CreatePullRequestCommentHandler : ICommandHandler<CreatePullRequestComment>
{
    private readonly IContextAccessor _contextAccessor;
    private readonly IMessageBroker _messageBroker;
    private readonly IPullRequestActionRequestStorage _pullRequestActionRequestStorage;
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public CreatePullRequestCommentHandler(IRepositoryRepository repositoryRepository, IContextAccessor contextAccessor,
        IMessageBroker messageBroker, IPullRequestRepository pullRequestRepository,
        IPullRequestActionRequestStorage pullRequestActionRequestStorage)
    {
        _repositoryRepository = repositoryRepository;
        _contextAccessor = contextAccessor;
        _messageBroker = messageBroker;
        _pullRequestRepository = pullRequestRepository;
        _pullRequestActionRequestStorage = pullRequestActionRequestStorage;
    }

    public async Task HandleAsync(CreatePullRequestComment command, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        var pullRequest = await _pullRequestRepository.GetAsync(command.RepositoryId, command.PullRequestId);
        if (pullRequest is null) throw new PullRequestNotFoundException(command.RepositoryId, command.PullRequestId);

        var pullRequestAction = new PullRequestAction(DateTime.Now, PullRequestActionType.Comment, command.Message,
            Array.Empty<string>(), _contextAccessor.Context.GetUserId());

        pullRequest.AddAction(pullRequestAction);

        await _pullRequestRepository.UpdateAsync(repository.Id, pullRequest);

        _pullRequestActionRequestStorage.SetPullRequestAction(command.ReferenceId, pullRequestAction);

        await _messageBroker.SendAsync(new PullRequestCommentCreated(pullRequestAction, repository.Id,
            pullRequest.Id), cancellationToken);
    }
}