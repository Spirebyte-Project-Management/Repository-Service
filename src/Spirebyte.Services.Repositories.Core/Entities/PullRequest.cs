using System;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class PullRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PullRequestStatus Status { get; set; }
    public PullRequestActions[] Actions { get; set; }
    public string Head { get; set; }
    public string Branch { get; set; }
    public DateTime CreatedAt { get; set; }
}