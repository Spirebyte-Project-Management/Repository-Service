using System;
using System.Runtime.Serialization;
using Spirebyte.Services.Repositories.Application.Exceptions.Base;

namespace Spirebyte.Services.Repositories.Application.Repositories.Exceptions;

[Serializable]
public class KeyAlreadyExistsException : AppException
{
    public KeyAlreadyExistsException(string repositoryId)
        : base($"Repository with id: {repositoryId} already exists.")
    {
        RepositoryId = repositoryId;
    }
    protected KeyAlreadyExistsException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    {
    }
    public override string Code { get; } = "key_already_exists";
    public string RepositoryId { get; }
}