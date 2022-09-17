using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Message("repositories", "delete_repository", "repositories.delete_repository")]
public class DeleteRepository : ICommand
{
    public DeleteRepository(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}