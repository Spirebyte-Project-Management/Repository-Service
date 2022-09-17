using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands;

[Message("repositories", "create_pull_request", "repositories.create_pull_request")]
public class CreatePullRequest : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();

    public CreatePullRequest(string repositoryId, string name, string description, PullRequestStatus status,
        PullRequestAction[] actions,
        string head, string branch, DateTime createdAt)
    {
        RepositoryId = repositoryId;
        Name = name;
        Description = description;
        Status = status;
        Actions = actions;
        Head = head;
        Branch = branch;
        CreatedAt = createdAt;
    }

    public string RepositoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PullRequestStatus Status { get; set; }
    public PullRequestAction[] Actions { get; set; }
    public string Head { get; set; }
    public string Branch { get; set; }
    public DateTime CreatedAt { get; set; }
}