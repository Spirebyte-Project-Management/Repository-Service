using System;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Contract]
public record CreateRepository(string Title, string Description, string ProjectId) : ICommand
{
    public DateTime CreatedAt { get; } = DateTime.Now;
    public Guid ReferenceId { get; } = Guid.NewGuid();
}