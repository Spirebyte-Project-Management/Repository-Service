using System;
using System.Collections;
using System.Collections.Generic;
using Spirebyte.Services.Repositories.Core.Enums;
using Spirebyte.Services.Repositories.Core.Exceptions;

namespace Spirebyte.Services.Repositories.Core.Entities;

public class Repository
{
    public Repository(string id, string title, string description, string projectId, Guid referenceId,
        List<Branch> branches, DateTime createdAt)
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
        CreatedAt = createdAt;
    }

    public Guid ChangeReferenceId()
    {
        ReferenceId = Guid.NewGuid();

        return ReferenceId;
    }
 
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    
    public Guid ReferenceId { get; set; }
    
    public List<Branch> Branches { get; set; }
    public DateTime CreatedAt { get; set; }
}