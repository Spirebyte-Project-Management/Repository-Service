using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Spirebyte.Framework.Messaging.Brokers;
using Spirebyte.Framework.Shared.Handlers;
using Spirebyte.Services.Repositories.Application.Repositories.Commands;
using Spirebyte.Services.Repositories.Application.Repositories.Commands.Handlers;
using Spirebyte.Services.Repositories.Application.Repositories.Events;
using Spirebyte.Services.Repositories.Application.Repositories.Exceptions;
using Spirebyte.Services.Repositories.Core.Repositories;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Application.Repository.Commands;

public class DeleteRepositoryHandlerTests
{
    private readonly ICommandHandler<DeleteRepository> _handler;
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;

    public DeleteRepositoryHandlerTests()
    {
        _repositoryRepository = Substitute.For<IRepositoryRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new DeleteRepositoryHandler(_repositoryRepository, _messageBroker);
    }

    [Fact]
    public async Task given_valid_command_delete_repository_should_succeed()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var command =
            new DeleteRepository(fakedRepository.Id);

        _repositoryRepository.ExistsAsync(fakedRepository.Id).Returns(true);
        _repositoryRepository.GetAsync(fakedRepository.Id).Returns(fakedRepository);

        await _messageBroker
            .SendAsync(Arg.Do<RepositoryDeleted>(r =>
            {
                r.Should().NotBeNull();
                r.Id.Should().Be(fakedRepository.Id);
                r.Title.Should().Be(fakedRepository.Title);
                r.Description.Should().Be(fakedRepository.Description);
                r.ProjectId.Should().Be(fakedRepository.ProjectId);
                r.CreatedAt.Should().BeCloseTo(fakedRepository.CreatedAt, TimeSpan.FromMinutes(1));
            }));

        await _handler
            .Awaiting(c => c.HandleAsync(command))
            .Should().NotThrowAsync();


        await _repositoryRepository.Received().DeleteAsync(fakedRepository.Id);
    }

    [Fact]
    public async Task delete_repository_should_fail_when_repository_does_not_exist()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var command =
            new DeleteRepository(fakedRepository.Id);


        await _handler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<RepositoryNotFoundException>();
    }
}