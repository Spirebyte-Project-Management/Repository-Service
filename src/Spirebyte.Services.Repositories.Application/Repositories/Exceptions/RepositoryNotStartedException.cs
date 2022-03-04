using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

public class RepositoryNotStartedException : AppException
{
    public RepositoryNotStartedException(string repositoryId) : base(
        $"Repository with id: '{repositoryId}' is not yet started.")
    {
        repositoryId = repositoryId;
    }

    public override string Code { get; } = "Repository_not_started";
    public string RepositoryId { get; }
}