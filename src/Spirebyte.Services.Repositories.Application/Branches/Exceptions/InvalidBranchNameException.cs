using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Branches.Exceptions;

public class InvalidBranchNameException : AppException
{
    public InvalidBranchNameException(string key) : base($"Branch title: '{key}' is invalid.")
    {
        Key = key;
    }

    public string Code { get; } = "invalid_branch_name";
    public string Key { get; }
}