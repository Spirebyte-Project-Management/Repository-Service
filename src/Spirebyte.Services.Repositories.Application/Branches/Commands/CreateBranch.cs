using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands;

[Contract]
public record CreateBranch(string Title, string BranchHead, string RepositoryId) : ICommand
{
    public Guid ReferenceId { get; } = Guid.NewGuid();
}