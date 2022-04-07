using System;
using Convey.CQRS.Commands;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Enums;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands;

[Contract]
public record CreatePullRequest(string RepositoryId, string Name, string Description, PullRequestStatus Status, PullRequestActions[] Actions,
    string Head, string Branch, DateTime CreatedAt) : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();
};
