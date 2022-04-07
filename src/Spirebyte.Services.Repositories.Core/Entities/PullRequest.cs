using System;
using System.Collections.Generic;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class PullRequest
{
    public PullRequest(long id, string name, string description, PullRequestStatus status, List<PullRequestActions> actions, string head, string branch, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
        Actions = actions;
        Head = head;
        Branch = branch;
        CreatedAt = createdAt;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PullRequestStatus Status { get; set; }
    public List<PullRequestActions> Actions { get; set; }
    public string Head { get; set; }
    public string Branch { get; set; }
    public DateTime CreatedAt { get; set; }
}