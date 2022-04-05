using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.Branches.Exceptions;

public class InvalidBranchNameException : AppException
{
    public InvalidBranchNameException(string key) : base($"Branch title: '{key}' is invalid.")
    {
        Key = key;
    }

    public override string Code { get; } = "invalid_branch_name";
    public string Key { get; }
}