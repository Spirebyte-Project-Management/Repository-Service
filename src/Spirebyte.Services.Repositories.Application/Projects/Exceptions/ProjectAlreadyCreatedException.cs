using Spirebyte.Framework.Shared.Exceptions;

namespace Spirebyte.Services.Repositories.Application.Projects.Exceptions;

public class ProjectAlreadyCreatedException : AppException
{
    public ProjectAlreadyCreatedException(string projectId)
        : base($"Project with id: {projectId} was already created.")
    {
        ProjectId = projectId;
    }

    public string Code { get; } = "project_already_created";
    public string ProjectId { get; }
}