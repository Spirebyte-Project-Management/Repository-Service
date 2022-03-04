﻿using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Repositories.API;
using Spirebyte.Services.Repositories.Application.Projects.Events.External;
using Spirebyte.Services.Repositories.Application.Projects.Exceptions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents;
using Spirebyte.Services.Repositories.Infrastructure.Mongo.Documents.Mappers;
using Spirebyte.Services.Repositories.Tests.Shared.Factories;
using Spirebyte.Services.Repositories.Tests.Shared.Fixtures;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Integration.Events;

[Collection("Spirebyte collection")]
public class ProjectCreatedTests : IDisposable
{
    private const string Exchange = "Repositories";
    private readonly IEventHandler<ProjectCreated> _eventHandler;
    private readonly MongoDbFixture<ProjectDocument, string> _projectMongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;

    public ProjectCreatedTests(SpirebyteApplicationFactory<Program> factory)
    {
        _rabbitMqFixture = new RabbitMqFixture();
        _projectMongoDbFixture = new MongoDbFixture<ProjectDocument, string>("projects");
        factory.Server.AllowSynchronousIO = true;
        _eventHandler = factory.Services.GetRequiredService<IEventHandler<ProjectCreated>>();
    }

    public async void Dispose()
    {
        _projectMongoDbFixture.Dispose();
    }


    [Fact]
    public async Task project_created_event_should_add_project_with_given_data_to_database()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().NotThrowAsync();


        var project = await _projectMongoDbFixture.GetAsync(projectId);

        project.Should().NotBeNull();
        project.Id.Should().Be(projectId);
    }

    [Fact]
    public async Task project_created_event_fails_when_project_with_id_already_exists()
    {
        var projectId = "projectKey" + Guid.NewGuid();

        var project = new Project(projectId);
        await _projectMongoDbFixture.InsertAsync(project.AsDocument());

        var externalEvent = new ProjectCreated { Id = projectId };

        // Check if exception is thrown

        await _eventHandler
            .Awaiting(c => c.HandleAsync(externalEvent))
            .Should().ThrowAsync<ProjectAlreadyCreatedException>();
    }
}