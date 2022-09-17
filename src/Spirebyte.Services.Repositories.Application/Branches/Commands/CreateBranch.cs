using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands;

[Message("repositories", "create_branch", "repositories.create_branch")]
public class CreateBranch : ICommand
{
    public CreateBranch(string title, string branchHead, string repositoryId)
    {
        Title = title;
        BranchHead = branchHead;
        RepositoryId = repositoryId;
    }

    public Guid ReferenceId { get; } = Guid.NewGuid();
    public string Title { get; set; }
    public string BranchHead { get; set; }
    public string RepositoryId { get; set; }
}