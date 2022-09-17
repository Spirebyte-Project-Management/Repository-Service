using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Message("repositories", "repository_deleted")]
internal class RepositoryDeleted : IEvent
{
    public RepositoryDeleted(string id, string title, string description,
        string projectId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        CreatedAt = createdAt;
    }

    public RepositoryDeleted(Repository repository)
    {
        Id = repository.Id;
        Title = repository.Title;
        Description = repository.Description;
        ProjectId = repository.ProjectId;
        CreatedAt = repository.CreatedAt;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}