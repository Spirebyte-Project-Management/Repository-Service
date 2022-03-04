using Convey.CQRS.Events;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Contract]
internal record AddedIssueToRepository(string RepositoryId, string ProjectId, string IssueId) : IEvent;