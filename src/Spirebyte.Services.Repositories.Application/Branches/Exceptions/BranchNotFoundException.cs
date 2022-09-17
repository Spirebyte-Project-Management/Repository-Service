using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Branches.Exceptions;

public class BranchNotFoundException : AppException
{
    public BranchNotFoundException(string name) : base($"Branch with name: '{name}' not found.")
    {
        Name = name;
    }

    public string Code { get; } = "branch_not_found";
    public string Name { get; }
}