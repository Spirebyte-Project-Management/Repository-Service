using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Shared.Changes;
using Spirebyte.Shared.Changes.ValueObjects;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Message("repositories", "repository_updated")]
internal class RepositoryUpdated : IEvent
{
    public RepositoryUpdated(string id, string title, string description,
        string projectId, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        CreatedAt = createdAt;
    }

    public RepositoryUpdated(Repository repository, Repository old)
    {
        Id = repository.Id;
        Title = repository.Title;
        Description = repository.Description;
        ProjectId = repository.ProjectId;
        CreatedAt = repository.CreatedAt;

        Changes = ChangedFieldsHelper.GetChanges(old, repository);
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Change[] Changes { get; set; }
}