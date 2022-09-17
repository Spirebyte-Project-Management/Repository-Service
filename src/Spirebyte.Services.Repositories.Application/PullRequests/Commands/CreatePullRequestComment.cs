using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands;

[Message("repositories", "create_pull_request_comment", "repositories.create_pull_request_comment")]
public class CreatePullRequestComment : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();

    public CreatePullRequestComment(string repositoryId, long pullRequestId, string message)
    {
        RepositoryId = repositoryId;
        PullRequestId = pullRequestId;
        Message = message;
    }

    public string RepositoryId { get; set; }
    public long PullRequestId { get; set; }
    public string Message { get; set; }
}