using System;
using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Message("repositories", "create_repository", "repositories.create_repository")]
public record CreateRepository(string Title, string Description, string ProjectId) : ICommand
{
    public DateTime CreatedAt { get; } = DateTime.Now;
    public Guid ReferenceId { get; } = Guid.NewGuid();
}