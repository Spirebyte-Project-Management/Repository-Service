using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.PullRequests.Commands;

public record CreatePullRequestComment(string RepositoryId, int PullRequestId, string Message) : ICommand
{
    public Guid ReferenceId = Guid.NewGuid();
};
