using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Events;

[Message("repositories", "pull_request_comment_created")]
public class PullRequestCommentCreated : IEvent
{
    public PullRequestCommentCreated(DateTime createdAt, PullRequestActionType type, string message, string[] commits,
        Guid userId, string repositoryId, int pullRequestId)
    {
        RepositoryId = repositoryId;
        PullRequestId = pullRequestId;
        CreatedAt = createdAt;
        Type = type;
        Message = message;
        Commits = commits;
        UserId = userId;
    }

    public PullRequestCommentCreated(PullRequestAction action, string repositoryId, long pullRequestId)
    {
        RepositoryId = repositoryId;
        PullRequestId = pullRequestId;
        CreatedAt = action.CreatedAt;
        Type = action.Type;
        Message = action.Message;
        Commits = action.Commits;
        UserId = action.UserId;
    }

    public DateTime CreatedAt { get; set; }
    public PullRequestActionType Type { get; set; }
    public string Message { get; set; }
    public string[] Commits { get; set; }
    public Guid UserId { get; set; }

    public string RepositoryId { get; set; }
    public long PullRequestId { get; set; }
}