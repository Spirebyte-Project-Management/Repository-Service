using System;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.Application.Background.Interfaces;
using Spirebyte.Services.Repositories.Application.Repositories.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.Repositories.Events.Handlers;

public class GitRepoUpdatedHandler : IEventHandler<GitRepoUpdated>
{
    private readonly IRepositoryService _repositoryService;
    private readonly IRepositoryRepository _repositoryRepository;

    public GitRepoUpdatedHandler(IRepositoryService repositoryService, IRepositoryRepository repositoryRepository)
    {
        _repositoryService = repositoryService;
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task HandleAsync(GitRepoUpdated @event, CancellationToken cancellationToken = default)
    {
        /*_queue.QueueBackgroundWorkItem(async token =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            
            var scopedServices = scope.ServiceProvider;
            var repositoryService = scopedServices.GetService<IRepositoryService>();
            var repositoryRepository = scopedServices.GetService<IRepositoryRepository>();

            if (repositoryService != null && repositoryRepository != null)
            {
                var repository = await repositoryService.UploadRepoChanges(@event.Repository);
                await repositoryRepository.UpdateAsync(repository);
            }
        });  
        
        return Task.CompletedTask;*/
        
        var repository = await _repositoryService.UploadRepoChanges(@event.Repository);
        await _repositoryRepository.UpdateAsync(repository);
    }
}