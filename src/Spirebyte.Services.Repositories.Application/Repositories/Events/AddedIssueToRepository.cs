using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Message("repositories", "added_issue_to_repository")]
internal record AddedIssueToRepository(string RepositoryId, string ProjectId, string IssueId) : IEvent;