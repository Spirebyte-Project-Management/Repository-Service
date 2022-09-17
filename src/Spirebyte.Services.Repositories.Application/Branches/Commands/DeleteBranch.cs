using Spirebyte.Framework.Shared.Abstractions;
using Spirebyte.Framework.Shared.Attributes;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands;

[Message("repositories", "delete_branch", "repositories.delete_branch")]
public record DeleteBranch(string BranchId, string RepositoryId) : ICommand;