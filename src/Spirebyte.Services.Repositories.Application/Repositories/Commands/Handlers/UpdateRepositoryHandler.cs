using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Exceptions;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;

internal sealed class UpdateRepositoryHandler : ICommandHandler<UpdateRepository>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;

    public UpdateRepositoryHandler(IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker)
    {
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(UpdateRepository command, CancellationToken cancellationToken = default)
    {
        var repository = await _repositoryRepository.GetAsync(command.Id);
        if (repository is null) throw new RepositoryNotFoundException(command.Id);

        var newRepository = new Repository(repository.Id, command.Title, command.Description, repository.ProjectId,
            repository.ReferenceId, repository.Branches, repository.CreatedAt);
        await _repositoryRepository.UpdateAsync(newRepository);

        await _messageBroker.PublishAsync(new RepositoryUpdated(newRepository, repository));
    }
}