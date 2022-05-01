using System;
using System.Collections.Generic;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class PullRequest
{
    public PullRequest()
    {
        
    }
    public PullRequest(long id, string name, string description, PullRequestStatus status, List<PullRequestAction> actions, string head, string branch, Guid userId, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
        Actions = actions;
        Head = head;
        Branch = branch;
        UserId = userId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PullRequestStatus Status { get; set; }
    public List<PullRequestAction> Actions { get; set; }
    public string Head { get; set; }
    public string Branch { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void AddAction(PullRequestAction action)
    {
        Actions.Add(action);
        UpdatedAt = DateTime.Now;
    }
}