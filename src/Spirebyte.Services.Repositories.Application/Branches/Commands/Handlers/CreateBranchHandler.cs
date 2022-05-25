using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using LibGit2Sharp;
using Spirebyte.Services.Repositories.Application.Branches.Events;
using Spirebyte.Services.Repositories.Application.Branches.Exceptions;
using Spirebyte.Services.Repositories.Application.Branches.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Clients.Interfaces;
using Spirebyte.Services.Repositories.Application.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Constants;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Shared.Contexts.Interfaces;
using RepositoryNotFoundException =
    Spirebyte.Services.Repositories.Application.Repositories.Exceptions.RepositoryNotFoundException;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands.Handlers;

internal sealed class CreateBranchHandler : ICommandHandler<CreateBranch>
{
    private const string BranchNameRegex =
        @"[^\000-\037\177 ~^:?*[]+(?<!\.lock)(?<!\/)(?<!\.)$";

    private readonly IAppContext _appContext;
    private readonly IBranchRequestStorage _branchRequestStorage;
    private readonly IEventDispatcher _eventDispatcher;

    private readonly IMessageBroker _messageBroker;
    private readonly IProjectsApiHttpClient _projectsApiHttpClient;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public CreateBranchHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker, IBranchRequestStorage branchRequestStorage, IEventDispatcher eventDispatcher,
        IProjectsApiHttpClient projectsApiHttpClient, IAppContext appContext)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
        _branchRequestStorage = branchRequestStorage;
        _eventDispatcher = eventDispatcher;
        _projectsApiHttpClient = projectsApiHttpClient;
        _appContext = appContext;
    }

    public async Task HandleAsync(CreateBranch command, CancellationToken cancellationToken = default)
    {
        // get repo
        var repository = await _repositoryRepository.GetAsync(command.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(command.RepositoryId);

        if (!await _projectsApiHttpClient.HasPermission(RepositoryPermissionKeys.CreateBranches,
                _appContext.Identity.Id,
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
        await _messageBroker.PublishAsync(new BranchCreated(createdBranch));
        _branchRequestStorage.SetBranch(command.ReferenceId, createdBranch);

        await _eventDispatcher.PublishAsync(new GitRepoUpdated(repository), cancellationToken);
    }
}