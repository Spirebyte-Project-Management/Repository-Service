using System.Threading.Tasks;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Core.Repositories;

public interface IPullRequestRepository
{
    Task<PullRequest> GetAsync(string repositoryId, int pullRequestId);
    Task<long> GetPullRequestCountOfRepositoryAsync(string repositoryId);
    Task<bool> ExistsAsync(string repositoryId, int pullRequestId);
    Task AddAsync(string repositoryId, PullRequest pullRequest);
    Task UpdateAsync(string repositoryId, PullRequest pullRequest);

    Task DeleteAsync(string repositoryId, int pullRequestId);
}