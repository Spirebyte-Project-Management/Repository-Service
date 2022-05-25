using System;
using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Events;

public class PullRequestCreated : IEvent
{
    public PullRequestCreated(long id, string name, string description, PullRequestStatus status,
        List<PullRequestAction> actions, string head, string branch, DateTime createdAt, string repositoryId)
    {
        Id = id;
        RepositoryId = repositoryId;
        Name = name;
        Description = description;
        Status = status;
        Actions = actions;
        Head = head;
        Branch = branch;
        CreatedAt = createdAt;
    }

    public PullRequestCreated(PullRequest pullRequest, string repositoryId)
    {
        Id = pullRequest.Id;
        RepositoryId = repositoryId;
        Name = pullRequest.Name;
        Description = pullRequest.Description;
        Status = pullRequest.Status;
        Actions = pullRequest.Actions;
        Head = pullRequest.Head;
        Branch = pullRequest.Branch;
        CreatedAt = pullRequest.CreatedAt;
    }

    public long Id { get; set; }
    public string RepositoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PullRequestStatus Status { get; set; }
    public List<PullRequestAction> Actions { get; set; }
    public string Head { get; set; }
    public string Branch { get; set; }
    public DateTime CreatedAt { get; set; }
}