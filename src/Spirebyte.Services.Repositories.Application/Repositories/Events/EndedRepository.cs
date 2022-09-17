using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events;

[Message("repositories", "ended_repository")]
internal record EndedRepository(string RepositoryId, string ProjectId) : IEvent;