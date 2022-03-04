using Convey.CQRS.Events;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Contract]
internal record RemovedIssueFromRepository(string RepositoryId, string ProjectId, string IssueId) : IEvent;