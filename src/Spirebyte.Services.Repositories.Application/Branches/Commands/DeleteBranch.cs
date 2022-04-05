using Convey.CQRS.Commands;

namespace Spirebyte.Services.Repositories.Application.Branches.Commands;

[Contract]
public record DeleteBranch(string BranchId, string RepositoryId) : ICommand;