using System;
using System.Collections.Generic;
using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Core.Exceptions;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class RepositoryTests
{
    [Fact]
    public void given_valid_input_Repository_should_be_created()
    {
        var repositoryId = "RepositoryKey";
        var title = "Title";
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        var repository = new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);

        repository.Should().NotBeNull();
        repository.Id.Should().Be(repositoryId);
        repository.Title.Should().Be(title);
        repository.Description.Should().Be(description);
        repository.ProjectId.Should().Be(projectId);
        repository.CreatedAt.Should().Be(createdAt);
    }


    [Fact]
    public void given_empty_id_Repository_should_throw_an_exception()
    {
        var repositoryId = string.Empty;
        var title = "Title";
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        Action act = () => new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidIdException>();
    }

    [Fact]
    public void given_empty_projectId_Repository_should_throw_an_exception()
    {
        var repositoryId = "RepositoryKey";
        var title = "Title";
        var description = "description";
        var projectId = string.Empty;
        var createdAt = DateTime.Now;

        Action act = () => new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidProjectIdException>();
    }

    [Fact]
    public void given_empty_title_project_should_throw_an_exception()
    {
        var repositoryId = "RepositoryKey";
        var title = string.Empty;
        var description = "description";
        var projectId = "projectKey";
        var createdAt = DateTime.Now;

        Action act = () => new Repository(repositoryId, title, description, projectId, new List<Branch>(), createdAt);
        act.Should().Throw<InvalidTitleException>();
    }
}