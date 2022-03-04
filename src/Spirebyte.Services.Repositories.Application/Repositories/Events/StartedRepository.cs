using Convey.CQRS.Events;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Contract]
internal record StartedRepository(string RepositoryId, string ProjectId) : IEvent;