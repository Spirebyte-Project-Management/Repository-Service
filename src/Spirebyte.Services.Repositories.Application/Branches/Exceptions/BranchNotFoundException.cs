using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.Branches.Exceptions;

public class BranchNotFoundException : AppException
{
    public BranchNotFoundException(string name) : base($"Branch with name: '{name}' not found.")
    {
        Name = name;
    }

    public override string Code { get; } = "branch_not_found";
    public string Name { get; }
}