using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class ProjectTests
{
    [Fact]
    public void given_valid_input_project_should_be_created()
    {
        var fakedProject = ProjectFaker.Instance.Generate();

        var project = new Project(fakedProject.Id);

        project.Should().NotBeNull();
        project.Id.Should().Be(fakedProject.Id);
    }
}