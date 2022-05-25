using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using LibGit2Sharp;
using Spirebyte.Services.Repositories.Application.Branches.Events;
using Spirebyte.Services.Repositories.Application.Branches.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using Branch = Spirebyte.Services.Repositories.Core.Entities.Branch;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands.Handlers;

internal sealed class DeleteBranchHandler : ICommandHandler<DeleteBranch>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public DeleteBranchHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IEventDispatcher eventDispatcher)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _eventDispatcher = eventDispatcher;
    }

    public async Task HandleAsync(DeleteBranch command, CancellationToken cancellationToken = default)
    {
        // get repo
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        // check branch name
        // get actual repo
        await _repositoryService.EnsureLatestRepositoryIsCached(repository);

        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);
        var repo = new Repository(repoPath);

        // check if branch exists
        var branch = repo.Branches.FirstOrDefault(b => b.CanonicalName == command.BranchId);
        if (branch is null) throw new BranchNotFoundException(command.BranchId);

        // delete branch
        repo.Branches.Remove(branch);

        await repository.UpdateRepositoryFromGit();
        await _repositoryRepository.UpdateAsync(repository);

        await _eventDispatcher.PublishAsync(new GitRepoUpdated(repository), cancellationToken);

        await _messageBroker.PublishAsync(new BranchDeleted(new Branch(branch)));

        await _eventDispatcher.PublishAsync(new GitRepoUpdated(repository), cancellationToken);
    }
}