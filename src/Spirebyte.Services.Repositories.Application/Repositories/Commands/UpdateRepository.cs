using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Message("repositories", "update_repository", "repositories.update_repository")]
public class UpdateRepository : ICommand
{
    public UpdateRepository(string id, string title, string description, string projectId)
    {
        Id = id;
        Title = title;
        Description = description;
        ProjectId = projectId;
        CreatedAt = DateTime.Now;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
}