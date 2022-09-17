using System;
using System.Runtime.Serialization;
using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Core.Exceptions;

[Serializable]
public class InvalidProjectIdException : DomainException
{
    public InvalidProjectIdException(string projectId) : base($"Invalid project id: {projectId}.")
    {
        ProjectId = projectId;
    }

    public string ProjectId { get; }
    public string Code { get; } = "invalid_project_id";
}