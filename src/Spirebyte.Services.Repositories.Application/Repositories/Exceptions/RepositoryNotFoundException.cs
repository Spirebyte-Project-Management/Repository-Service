using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

public class RepositoryNotFoundException : AppException
{
    public RepositoryNotFoundException(string key) : base($"Repository with Key: '{key}' was not found.")
    {
        Key = key;
    }

    public override string Code { get; } = "Repository_not_found";
    public string Key { get; }
}