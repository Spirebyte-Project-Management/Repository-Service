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

public class UpdateRepositoryHandlerTests
{
    private readonly ICommandHandler<UpdateRepository> _handler;
    private readonly IMessageBroker _messageBroker;
    private readonly IRepositoryRepository _repositoryRepository;

    public UpdateRepositoryHandlerTests()
    {
        _repositoryRepository = Substitute.For<IRepositoryRepository>();
        _messageBroker = Substitute.For<IMessageBroker>();
        _handler = new UpdateRepositoryHandler(_repositoryRepository, _messageBroker);
    }

    [Fact]
    public async Task given_valid_command_update_repository_should_succeed()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var command = new UpdateRepository(fakedRepository.Id, fakedRepository.Title, fakedRepository.Description,
            fakedRepository.ProjectId);

        _repositoryRepository.GetAsync(fakedRepository.Id).Returns(fakedRepository);

        await _messageBroker
            .SendAsync(Arg.Do<RepositoryUpdated>(r =>
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


        await _repositoryRepository.Received().UpdateAsync(Arg.Do<Repositories.Core.Entities.Repository>(r =>
        {
            r.Should().NotBeNull();
            r.Id.Should().Be(fakedRepository.Id);
            r.Title.Should().Be(fakedRepository.Title);
            r.Description.Should().Be(fakedRepository.Description);
            r.ProjectId.Should().Be(fakedRepository.ProjectId);
            r.CreatedAt.Should().BeCloseTo(fakedRepository.CreatedAt, TimeSpan.FromMinutes(1));
        }));
    }

    [Fact]
    public async Task update_repository_should_fail_when_repository_does_not_exist()
    {
        var fakedRepository = RepositoryFaker.Instance.Generate();

        var command = new UpdateRepository(fakedRepository.Id, fakedRepository.Title, fakedRepository.Description,
            fakedRepository.ProjectId);

        await _handler
            .Awaiting(c => c.HandleAsync(command))
            .Should().ThrowAsync<RepositoryNotFoundException>();
    }
}