using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Exceptions;
using Spirebyte.Services.Repositories.Application.Services.Interfaces;
using Spirebyte.Services.Repositories.Core.Repositories;

namespace Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;

internal sealed class DeleteRepositoryHandler : ICommandHandler<DeleteRepository>
{
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;

    public DeleteRepositoryHandler(IRepositoryRepository repositoryRepository,
        IMessageBroker messageBroker)
    {
        _repositoryRepository = repositoryRepository;
        _messageBroker = messageBroker;
    }

    public async Task HandleAsync(DeleteRepository command, CancellationToken cancellationToken = default)
    {
        if (!await _repositoryRepository.ExistsAsync(command.Id)) throw new RepositoryNotFoundException(command.Id);

        var repository = await _repositoryRepository.GetAsync(command.Id);

        await _repositoryRepository.DeleteAsync(repository.Id);

        await _messageBroker.PublishAsync(new RepositoryDeleted(repository));
    }
}