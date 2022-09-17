using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Message("repositories", "removed_issue_from_repository")]
internal record RemovedIssueFromRepository(string RepositoryId, string ProjectId, string IssueId) : IEvent;