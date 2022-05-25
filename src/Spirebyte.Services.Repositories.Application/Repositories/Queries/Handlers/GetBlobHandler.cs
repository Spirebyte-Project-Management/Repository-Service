using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using LibGit2Sharp;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries.Handlers;

public class GetBlobHandler : IQueryHandler<GetBlob, BlobDto>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryService _repositoryService;

    public GetBlobHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
    }


    public async Task<BlobDto> HandleAsync(GetBlob query, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(query.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(query.RepositoryId);

        await _repositoryService.EnsureLatestRepositoryIsCached(repository);

        var repo = new Repository(RepoPathHelpers.GetCachePathForRepository(repository));

        if (!repo.Commits.Any()) throw new NotFoundException();

        Commit searchCommit = null;
        if (!string.IsNullOrWhiteSpace(query.CommitSha)) searchCommit = repo.Lookup<Commit>(query.CommitSha);

        if (!string.IsNullOrWhiteSpace(query.Branch)) searchCommit = repo.Branches[query.Branch].Tip;

        // Fallback to latest commit of default branch
        if (searchCommit == null) searchCommit = repo.Head.Tip;

        // File trees are bound to commits
        // When no path is defined then we use the base tree of a commit
        var treeTarget = searchCommit[query.Path];
        // if not a file tree then return null
        if (treeTarget.TargetType != TreeEntryTargetType.Blob) return null;

        var blob = treeTarget.Target as Blob;

        var parentCommit = repo.Commits.QueryBy(new CommitFilter
        {
            IncludeReachableFrom = searchCommit,
            SortBy = CommitSortStrategies.Topological | CommitSortStrategies.Reverse
        }).FirstOrDefault(a => a[query.Path] != null && a[query.Path].Target.Sha == blob.Sha);

        return new BlobDto(blob, parentCommit, query.Path);
    }
}