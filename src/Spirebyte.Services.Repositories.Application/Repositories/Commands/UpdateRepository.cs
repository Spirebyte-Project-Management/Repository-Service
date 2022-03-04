using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Contract]
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