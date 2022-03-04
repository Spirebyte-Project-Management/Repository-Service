using Convey.CQRS.Events;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Contract]
internal record EndedRepository(string RepositoryId, string ProjectId) : IEvent;