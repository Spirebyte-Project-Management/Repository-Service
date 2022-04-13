using FluentAssertions;
using Spirebyte.Services.Repositories.Core.Entities;
using Spirebyte.Services.Repositories.Tests.Shared.MockData.Entities;
using Xunit;

namespace Spirebyte.Services.Repositories.Tests.Unit.Core.Entities;

public class BranchTests
{
    [Fact]
    public void given_valid_input_branch_should_be_created()
    {
        var fakedBranch = BranchFaker.Instance.Generate();

        var branch = new Branch(fakedBranch.Id, fakedBranch.Name, fakedBranch.IsHead,
            fakedBranch.Commits);

        branch.Should().NotBeNull();
        branch.Id.Should().Be(fakedBranch.Id);
        branch.Name.Should().Be(fakedBranch.Name);
        branch.IsHead.Should().Be(fakedBranch.IsHead);
        branch.Commits.Should().Contain(fakedBranch.Commits);
    }
}