using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

public class RepositoryNotFoundException : AppException
{
    public RepositoryNotFoundException(string key) : base($"Repository with Key: '{key}' was not found.")
    {
        Key = key;
    }

    public string Code { get; } = "Repository_not_found";
    public string Key { get; }
}