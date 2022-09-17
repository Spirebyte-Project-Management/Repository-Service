using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LibGit2Sharp;
using Spirebyte.Framework.Contexts;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Branches.Events;
using Spirebyte.Services.Repositories.Application.Branches.Exceptions;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using RepositoryNotFoundException =
    Spirebyte.Services.Repositories.Application.Repositories.Exceptions.RepositoryNotFoundException;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands.Handlers;

internal sealed class CreateBranchHandler : ICommandHandler<CreateBranch>
{
    private const string BranchNameRegex =
        @"[^\000-\037\177 ~^:?*[]+(?<!\.lock)(?<!\/)(?<!\.)$";

    private readonly IContextAccessor _contextAccessor;
    private readonly IBranchRequestStorage _branchRequestStorage;
    private readonly IEventDispatcher _eventDispatcher;

    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public CreateBranchHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IBranchRequestStorage branchRequestStorage, IEventDispatcher eventDispatcher,
        IProjectsApiHttpClient projectsApiHttpClient, IContextAccessor contextAccessor)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _branchRequestStorage = branchRequestStorage;
        _eventDispatcher = eventDispatcher;
        _projectsApiHttpClient = projectsApiHttpClient;
        _contextAccessor = contextAccessor;
    }

    public async Task HandleAsync(CreateBranch command, CancellationToken cancellationToken = default)
    {
        // get repo
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.CreateBranches,
                _contextAccessor.Context.GetUserId(),
                repository.ProjectId)) throw new ActionNotAllowedException();

        // check branch name
        if (!Regex.IsMatch(command.Title, BranchNameRegex)) throw new InvalidBranchNameException(command.Title);

        // get actual repo
        await _repositoryService.EnsureLatestRepositoryIsCached(repository);

        var repoPath = RepoPathHelpers.GetCachePathForRepository(repository);
        var repo = new Repository(repoPath);

        // check branch head
        var branchHead = repo.Branches.FirstOrDefault(b => b.CanonicalName == command.BranchHead);
        if (branchHead is null) throw new BranchNotFoundException(command.BranchHead);

        // create branch
        repo.CreateBranch(command.Title, branchHead.Tip);

        await repository.UpdateRepositoryFromGit();
        await _repositoryRepository.UpdateAsync(repository);

        var createdBranch = repository.Branches.FirstOrDefault(b => b.Name == command.Title);
        await _messageBroker.SendAsync(new BranchCreated(createdBranch), cancellationToken);
        _branchRequestStorage.SetBranch(command.ReferenceId, createdBranch);

        await _eventDispatcher.PublishAsync(new GitRepoUpdated(repository), cancellationToken);
    }
}