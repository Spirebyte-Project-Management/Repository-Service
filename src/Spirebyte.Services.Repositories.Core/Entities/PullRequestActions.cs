using System;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class PullRequestActions
{
    public DateTime CreatedAt { get; set; }
    public PullRequestActionType Type { get; set; }
    public string Message { get; set; }
    public string[] Commits { get; set; }
    public string UserId { get; set; }
}