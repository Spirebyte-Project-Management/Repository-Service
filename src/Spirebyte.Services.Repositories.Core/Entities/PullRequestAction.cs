using System;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class PullRequestAction
{
    public PullRequestAction(DateTime createdAt, PullRequestActionType type, string message, string[] commits, Guid userId)
    {
        CreatedAt = createdAt;
        Type = type;
        Message = message;
        Commits = commits;
        UserId = userId;
    }
    public DateTime CreatedAt { get; set; }
    public PullRequestActionType Type { get; set; }
    public string Message { get; set; }
    public string[] Commits { get; set; }
    public Guid UserId { get; set; }
}