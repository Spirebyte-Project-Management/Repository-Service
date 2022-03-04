using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.Projects.Events.External.Handlers;

internal sealed class ProjectCreatedHandler : IEventHandler<ProjectCreated>
{
    private readonly IProjectRepository _projectRepository;

    public ProjectCreatedHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task HandleAsync(ProjectCreated @event, CancellationToken cancellationToken = default)
    {
        if (await _projectRepository.ExistsAsync(@event.Id))
            throw new ProjectAlreadyCreatedException(@event.Id);

        var project = new Project(@event.Id);
        await _projectRepository.AddAsync(project);
    }
}