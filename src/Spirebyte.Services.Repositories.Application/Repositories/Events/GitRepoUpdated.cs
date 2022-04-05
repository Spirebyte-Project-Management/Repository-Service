using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Core.Entities;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

public class GitRepoUpdated : IEvent
{
    public GitRepoUpdated(Repository repository)
    {
        Repository = repository;
    }
    public Repository Repository { get; set; }
}