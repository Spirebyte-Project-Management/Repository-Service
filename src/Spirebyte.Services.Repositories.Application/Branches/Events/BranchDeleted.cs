using System.Collections.Generic;
using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Branches.Events;

[Contract]
internal class BranchDeleted : IEvent
{
    public BranchDeleted(string id, string name, bool isHead, List<Commit> commits)
    {
        Id = id;
        Name = name;
        IsHead = isHead;
        Commits = commits;
    }

    public BranchDeleted(Branch branch)
    {
        Id = branch.Id;
        Name = branch.Name;
        IsHead = branch.IsHead;
        Commits = branch.Commits;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsHead { get; set; }
    public List<Commit> Commits { get; set; }
}