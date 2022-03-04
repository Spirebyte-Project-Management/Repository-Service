using Convey.CQRS.Events;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events.Rejected;

[Contract]
public record RepositoryCreatedRejected(string RepositoryId, string Reason, string Code) : IRejectedEvent;