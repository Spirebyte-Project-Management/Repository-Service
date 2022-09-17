using System;
using System.Runtime.Serialization;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

[Serializable]
public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string repositoryId)
        : base($"Repository with id: {repositoryId} already exists.")
    {
        RepositoryId = repositoryId;
    }

    public string Code { get; } = "key_already_exists";
    public string RepositoryId { get; }
}