using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands;

[Contract]
public class DeleteRepository : ICommand
{
    public DeleteRepository(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}