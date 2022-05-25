using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Exceptions;

public class PullRequestNotFoundException : AppException
{
    public PullRequestNotFoundException(string repositoryId, int pullRequestId) : base(
        $"Pullrequest with id: '{pullRequestId}' was not found. In repository with id: {repositoryId}")
    {
        RepositoryId = repositoryId;
        PullRequestId = pullRequestId;
    }

    public override string Code { get; } = "pull_request_not_found";
    public string RepositoryId { get; }
    public int PullRequestId { get; }
}