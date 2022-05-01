using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spirebyte.Services.Repositories.Core.Exceptions;
using Spirebyte.Services.Repositories.Core.Helpers;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class Repository
{
    public Repository()
    {
        
    }
    public Repository(string id, string title, string description, string projectId, Guid referenceId,
        List<Branch> branches, List<PullRequest> pullRequests, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new InvalidIdException(id);

        if (string.IsNullOrWhiteSpace(projectId)) throw new InvalidProjectIdException(projectId);

        if (string.IsNullOrWhiteSpace(title)) throw new InvalidTitleException(title);

        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        ReferenceId = referenceId;
        Branches = branches;
        PullRequests = pullRequests;
        CreatedAt = createdAt;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }

    public Guid ReferenceId { get; set; }

    public List<Branch> Branches { get; set; }
    public List<PullRequest> PullRequests { get; set; }
    public DateTime CreatedAt { get; set; }

    public Task UpdateRepositoryFromGit()
    {
        var repoPath = RepoPathHelpers.GetCachePathForRepositoryId(Id);
        var repoInstance = new LibGit2Sharp.Repository(repoPath);
        Branches = repoInstance.Branches.Select(b => new Branch(b)).ToList();
        return Task.CompletedTask;
    }
    
    public Guid ChangeReferenceId()
    {
        ReferenceId = Guid.NewGuid();

        return ReferenceId;
    }
}