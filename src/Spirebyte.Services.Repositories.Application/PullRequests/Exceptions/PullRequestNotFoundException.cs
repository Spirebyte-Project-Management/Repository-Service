using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Exceptions;

public class PullRequestNotFoundException : AppException
{
    public PullRequestNotFoundException(string repositoryId, long pullRequestId) : base(
        $"Pullrequest with id: '{pullRequestId}' was not found. In repository with id: {repositoryId}")
    {
        RepositoryId = repositoryId;
        PullRequestId = pullRequestId;
    }

    public string Code { get; } = "pull_request_not_found";
    public string RepositoryId { get; }
    public long PullRequestId { get; }
}