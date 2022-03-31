using System;
using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Contract]
internal class RepositoryCreated : IEvent
{
    public RepositoryCreated(string id, string title, string description,
        string projectId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        CreatedAt = createdAt;
    }

    public RepositoryCreated(Repository repository)
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