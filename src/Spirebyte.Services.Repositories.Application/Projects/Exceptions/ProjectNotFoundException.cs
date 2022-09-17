using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Projects.Exceptions;

public class ProjectNotFoundException : AppException
{
    public ProjectNotFoundException(string projectId) : base($"Project with Id: '{projectId}' was not found.")
    {
        ProjectId = projectId;
    }

    public string Code { get; } = "project_not_found";
    public string ProjectId { get; }
}