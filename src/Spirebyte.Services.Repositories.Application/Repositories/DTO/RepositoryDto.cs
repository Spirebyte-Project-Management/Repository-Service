﻿using System;
using System.Collections.Generic;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.DTO;

public class RepositoryDto
{
    public RepositoryDto()
    {
    }

    public RepositoryDto(Repository repository)
    {
        Id = repository.Id;
        Title = repository.Title;
        Description = repository.Description;
        ProjectId = repository.ProjectId;
        Branches = repository.Branches;
        PullRequests = repository.PullRequests;
        CreatedAt = repository.CreatedAt;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public List<Branch> Branches { get; set; }
    public List<PullRequest> PullRequests { get; set; }
    public DateTime CreatedAt { get; set; }
}