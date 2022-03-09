using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using LibGit2Sharp;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.DTO;
using Spirebyte.Services.Repositories.Application.Repositories.Exceptions;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Helpers;
using Spirebyte.Services.Repositories.Core.Repositories;
using RepositoryNotFoundException = Spirebyte.Services.Repositories.Application.Repositories.Exceptions.RepositoryNotFoundException;

namespace Spirebyte.Services.Repositories.Application.Repositories.Queries.Handlers;

public class GetTreeHandler : IQueryHandler<GetTree, TreeDto>
{
    private readonly IRepositoryService _repositoryService;
    private readonly IRepositoryRepository _repositoryRepository;

    public GetTreeHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
    }


    public async Task<TreeDto> HandleAsync(GetTree query, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(query.RepositoryId);
        if (repository is null) throw new RepositoryNotFoundException(query.RepositoryId);

        await _repositoryService.EnsureLatestRepositoryIsCached(repository);

        var repo = new Repository(RepoPathHelpers.GetCachePathForRepository(repository));

        var path = string.IsNullOrEmpty(query.Path) ? "/" : query.Path;
        if (!repo.Commits.Any())
        {
            return new TreeDto(path);
        }

        Commit searchCommit = null;
        if (!string.IsNullOrWhiteSpace(query.CommitSha))
        {
            searchCommit = repo.Lookup<Commit>(query.CommitSha);
        }

        if (!string.IsNullOrWhiteSpace(query.Branch))
        {
            searchCommit = repo.Branches[query.Branch].Tip;
        }

        // Fallback to latest commit of default branch
        if (searchCommit == null)
        {
            searchCommit = repo.Head.Tip;
        }

        // File trees are bound to commits
        // When no path is defined then we use the base tree of a commit
        Tree tree;
        if (string.IsNullOrEmpty(query.Path))
        {
            tree = searchCommit.Tree;
        }
        else
        {
            var treeTarget = searchCommit[query.Path];
            // if not a file tree then return null
            if (treeTarget.TargetType != TreeEntryTargetType.Tree)
            {
                return null;
            }

            tree = treeTarget.Target as Tree;
        }


        var ancestors = repo.Commits.QueryBy(new CommitFilter { IncludeReachableFrom = searchCommit, SortBy = CommitSortStrategies.Topological | CommitSortStrategies.Reverse }).ToList();
        return new TreeDto(searchCommit, ancestors, tree, path);
    }
}