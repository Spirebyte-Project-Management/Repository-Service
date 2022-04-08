using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using LibGit2Sharp;
using Spirebyte.Services.Repositories.Application.PullRequests.Events;
using Spirebyte.Services.Repositories.Application.PullRequests.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands.Handler;

public class CreatePullRequestHandler : ICommandHandler<CreatePullRequest>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IPullRequestRequestStorage _pullRequestRequestStorage;

    public CreatePullRequestHandler(IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IPullRequestRepository pullRequestRepository, IPullRequestRequestStorage pullRequestRequestStorage)
    {
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _pullRequestRepository = pullRequestRepository;
        _pullRequestRequestStorage = pullRequestRequestStorage;
    }

    public async Task HandleAsync(CreatePullRequest command, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        var pullRequestCount = await _pullRequestRepository.GetPullRequestCountOfRepositoryAsync(command.RepositoryId);
        
        var newPullRequest = new PullRequest(pullRequestCount + 1, command.Name, command.Description, command.Status,
            new List<PullRequestAction>(), command.Head, command.Branch, DateTime.Now);
        await _pullRequestRepository.AddAsync(repository.Id, newPullRequest);

        _pullRequestRequestStorage.SetPullRequest(command.ReferenceId, newPullRequest);

        await _messageBroker.PublishAsync(new PullRequestCreated(newPullRequest, repository.Id));
    }
}